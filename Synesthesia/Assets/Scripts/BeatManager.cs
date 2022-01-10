using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{

    public float normalSpeed;
    public Drum snare;
    public Drum hiTom;
    public Drum midTom;
    public Drum floorTom;
    public Drum kick;
    public Drum hiHat;

    public string[] stageTwoCombos;
    public float[] stageTwoComboTimes; 
    public float[] stageTwoComboSpeeds; 
    public float nextPlayTime;
    private float stageStartTime; 
    private Drum curDrum;
    private int stage;
    private bool activated;

    private Dictionary<char, Drum> stringToDrumType = new Dictionary<char, Drum>();
    private int curComboIndex = 0;
    private int curComboTimeIndex = 0;
    private int curIndexInCombo = 0; 
    private bool inCombo;
    public float speed; 

    private static BeatManager _instance;

    public static BeatManager Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        stringToDrumType.Add('S', snare);
        stringToDrumType.Add('F', floorTom);
        stringToDrumType.Add('M', midTom);
        stringToDrumType.Add('H', hiTom);
        stringToDrumType.Add('C', hiHat);

        speed = normalSpeed; 
    }

    public void Activate()
    {
        stage = GameManager.Instance.GetGameStage(); 
        if(stage == 1)
        {
            curDrum = hiTom;
        }
        else if(stage == 2)
        {
            curDrum = snare; 
        }
        activated = true; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(activated)
        {
            if (stage == 1)
            {
                if (Time.time > nextPlayTime)
                {
                    curDrum.SpawnNote(isComboNote: false, isLastComboNote: false);

                    if (curDrum == hiTom)
                    {
                        curDrum = midTom;
                    }
                    else
                    {
                        curDrum = hiTom;
                    }

                    nextPlayTime = Time.time + speed;
                }
            }
            else if (stage == 2)
            {
                if(curComboTimeIndex < stageTwoComboTimes.Length && (Time.time > stageTwoComboTimes[curComboTimeIndex]))
                {
                    Debug.Log("Starting Combo!");
                    inCombo = true;
                    curComboTimeIndex += 1;
                    nextPlayTime = Time.time;
                    speed = stageTwoComboSpeeds[curComboTimeIndex]; 

                    // -- provide length of all notes in the upcombing combo
                    StageTwo.Instance.ComboStarted(stageTwoCombos[curComboIndex].Replace(" ", "").Length);
                }

                if(inCombo)
                {
                    if(Time.time >= nextPlayTime)
                    {
                        Debug.Log("Next combo beat");
                        char curDrumChar = stageTwoCombos[curComboIndex][curIndexInCombo];

                        while (curDrumChar != ' ')
                        {
                            // -- Index of last note 
                            if(curIndexInCombo == stageTwoCombos[curComboIndex].Length - 1)
                            {
                                stringToDrumType[curDrumChar].SpawnNote(isComboNote: true, isLastComboNote: true);
                                inCombo = false;
                                curComboIndex += 1;
                                curIndexInCombo = 0;
                                speed = normalSpeed; 
                                break;
                            }
                            // -- Not last note 
                            else
                            {
                                stringToDrumType[curDrumChar].SpawnNote(isComboNote: true, isLastComboNote: false);
                                curIndexInCombo += 1;
                            }
                            curDrumChar = stageTwoCombos[curComboIndex][curIndexInCombo];
                        }

                        // -- if combo is not over 
                        if(inCombo)
                        {
                            curIndexInCombo += 1; // -- so doesn't start on ' ' next beat 
                        }
                        nextPlayTime = Time.time + speed;    
                    }
                }

                else if (Time.time > nextPlayTime)
                {
                    curDrum.SpawnNote(isComboNote: false, isLastComboNote: false);

                    if (curDrum == snare)
                    {
                        curDrum = snare;
                    }
                    else
                    {
                        curDrum = snare;
                    }

                    nextPlayTime = Time.time + speed;
                }
            }
        }
    }
}
