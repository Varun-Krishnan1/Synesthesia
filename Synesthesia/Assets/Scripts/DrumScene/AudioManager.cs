using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class AudioManager : MonoBehaviour
{

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public AudioClip[] audioClips; 
    private AudioSource mainTheme; 


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
            mainTheme.time = 20f;
        }
        else if(stage == 2)
        {
            mainTheme.clip = audioClips[1];
        }
        else if(stage == 3)
        {
            mainTheme.clip = audioClips[2];
        }
        mainTheme.Play();
    }
}
