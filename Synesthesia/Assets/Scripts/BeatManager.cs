using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public float normalBeatPlayedOn; 

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
    public bool activated;

    private Dictionary<char, Drum> stringToDrumType = new Dictionary<char, Drum>();
    private int curComboIndex = 0;
    private int curComboTimeIndex = 0;
    private int curIndexInCombo = 0; 
    private bool inCombo;
    public float secPerBeat;

    private float nextBeat; 

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
    }

    public void Activate()
    {
        stage = GameManager.Instance.GetGameStage(); 
        if(stage == 1)
        {
            curDrum = snare;
        }
        else if(stage == 2)
        {
            curDrum = snare; 
        }

        activated = true; 
    }

    // Called from Update of AudioManager which uses AudioSettings.dspTime
    public void BeatChecker(float numBeats)
    {
        if(activated)
        {
            if (stage == 1)
            {
                //if (songPosition > nextPlayTime)
                if(numBeats % normalBeatPlayedOn == 0)
                {
                    curDrum.SpawnNote(isComboNote: false, isLastComboNote: false);

                    if (curDrum == snare)
                    {
                        curDrum = floorTom;
                    }
                    else
                    {
                        curDrum = snare;
                    }
                }
            }
            else if (stage == 2)
            //if(true)
            {
                Debug.Log(curComboTimeIndex);
                Debug.Log(numBeats);
                if(curComboTimeIndex < stageTwoComboTimes.Length && (numBeats == stageTwoComboTimes[curComboTimeIndex]))
                {
                    Debug.Log("Starting Combo!");
                    inCombo = true;

                    // -- provide length of all notes in the upcombing combo
                    StageTwo.Instance.ComboStarted(stageTwoCombos[curComboIndex].Replace(" ", "").Length);
                }

                if(inCombo)
                {
                    if(numBeats % stageTwoComboSpeeds[curComboTimeIndex] == 0)
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
                                curComboTimeIndex += 1;
                                curIndexInCombo = 0;
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
                    }
                }

                else if(numBeats % normalBeatPlayedOn == 0)
                {
                    curDrum.SpawnNote(isComboNote: false, isLastComboNote: false);

                    if (curDrum == snare)
                    {
                        curDrum = floorTom;
                    }
                    else
                    {
                        curDrum = snare;
                    }
                }          
            }
        }
    }
}
