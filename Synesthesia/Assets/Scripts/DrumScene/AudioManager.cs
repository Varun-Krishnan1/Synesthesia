using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class AudioManager : MonoBehaviour
{

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public AudioClip[] audioClips;
    public AudioClip[] soundEffectClips; 
    public AudioSource mainTheme;
    public AudioSource soundEffectTheme; 


    public float songBpm;
    public float secPerBeat;
    public float numBeats;
    
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
        mainTheme = GetComponent<AudioSource>(); 
    }

    public void ClearTheme()
    {
        mainTheme.clip = null; 
    }

    public void StartStageTheme(int stage)
    {
        if(stage == 0)
        {
            mainTheme.clip = audioClips[0];
        }
        else if(stage == 1)
        {
            mainTheme.clip = audioClips[0];
            soundEffectTheme.clip = soundEffectClips[0];
            mainTheme.time = 20f;
        }
        else if(stage == 2)
        {
            mainTheme.clip = audioClips[1];
            soundEffectTheme.clip = soundEffectClips[0];
            //soundEffectTheme.Play();
        }
        else if(stage == 3)
        {
            mainTheme.clip = audioClips[2];
            soundEffectTheme.clip = soundEffectClips[1];
            //soundEffectTheme.Play();
        }

        //Calculate the number of seconds in each beat
        secPerBeat = 60f / songBpm;
        //mainTheme.Play();

        InvokeRepeating("HalfBeatPassed", 0, secPerBeat / 4); 
    }

    // -- called every half beat 
    void HalfBeatPassed()
    {
        numBeats += 0.25f;
        BeatManager.Instance.BeatChecker(numBeats);
        if(StageTwo.Instance)
        {
            if(numBeats == StageTwo.Instance.startDelayInBeats)
            {
                StageTwo.Instance.StartGameplayLoop(); 
            }
            StageTwo.Instance.enemyShip.BeatChecker(numBeats); 
        }
    }
    void Update()
    {

    }
}
