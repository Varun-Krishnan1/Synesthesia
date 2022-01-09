using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatManager : MonoBehaviour
{

    public float speed;
    public Drum snare;
    public Drum hiTom;
    public Drum midTom;
    public Drum floorTom;
    public Drum kick;
    public Drum hiHat; 
   
    public float nextPlayTime;
    private Drum curDrum;
    private int stage;
    private bool activated; 

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

    // Start is called before the first frame update
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
    void Update()
    {
        if(activated)
        {
            if (stage == 1)
            {
                if (Time.time > nextPlayTime)
                {
                    curDrum.SpawnNote();

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
                if (Time.time > nextPlayTime)
                {
                    curDrum.SpawnNote();

                    if (curDrum == snare)
                    {
                        curDrum = floorTom;
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
