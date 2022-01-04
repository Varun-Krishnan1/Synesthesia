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
   
    private float nextPlayTime;
    private Drum curDrum;
    private int stage; 

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
    void Start()
    {
        curDrum = hiTom;
        stage = GameManager.Instance.GetGameStage(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(stage == 1)
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
        else if(stage == 2)
        {
            curDrum = snare; 

            if (Time.time > nextPlayTime)
            {
                curDrum.SpawnNote();

                nextPlayTime = Time.time + speed;
            }
        }
    }
}
