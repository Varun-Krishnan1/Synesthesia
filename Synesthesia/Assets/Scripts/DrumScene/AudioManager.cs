using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class AudioManager : MonoBehaviour
{

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public float delay; 

    private float startTime; 
    private float themeStartTime = 0f; 

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

    public float getCurrentTimePoint()
    {
        if(themeStartTime == 0f)
        {
            return 0f; 
        }
        return Time.time - themeStartTime; 
    }


    public void StartTheme()
    {
        AudioSource mainTheme = GetComponent<AudioSource>();
        // mainTheme.Play();
        themeStartTime = Time.time;
    }
}

/* 
 * 8 seconds - first transition; .25 seconds per beat 
 * 30 seconds - second transition; 
 */ 