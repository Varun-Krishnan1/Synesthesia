using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class AudioManager : MonoBehaviour
{

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public AudioClip[] mainThemeClips;
    public AudioClip[] mainThemeEffectsClips; 
    public AudioClip[] soundEffectClips; 
    public AudioSource mainTheme;
    public AudioSource mainThemeEffects; 

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
        mainTheme.Stop(); 
        mainTheme.clip = null; 
    }

    public void StartStageTheme(int stage)
    {

        // -- In case any invoking functions still going stop it 
        CancelInvoke("HalfBeatPassed");
        numBeats = 0; // -- reset beats as well 

        if (stage == 0)
        {
            mainTheme.clip = mainThemeClips[0];
            // mainTheme.time = 20f;

            mainTheme.Play();
        }
        else if(stage == 1)
        {
            mainThemeEffects.clip = mainThemeEffectsClips[0];
            mainThemeEffects.Play();

            InvokeRepeating("HalfBeatPassed", 0, secPerBeat / 4);
        }
        else if(stage == 2)
        {
            mainThemeEffects.Stop();

            mainTheme.clip = mainThemeClips[1];
            mainTheme.Play();

            //Calculate the number of seconds in each beat
            secPerBeat = 60f / songBpm;

            InvokeRepeating("HalfBeatPassed", 0, secPerBeat / 4);
        }
        else if(stage == 3)
        {
            mainThemeEffects.clip = mainThemeEffectsClips[1];
            mainThemeEffects.Play();

            mainTheme.clip = mainThemeClips[2];
            mainTheme.Play();

            //Calculate the number of seconds in each beat
            secPerBeat = 60f / songBpm;
        }


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

    public void PlaySoundEffect(int clipNum, float volume, float startTime)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = soundEffectClips[clipNum];
        audioSource.time = startTime;
        audioSource.volume = volume; 
        audioSource.Play(); 
    }
}
