using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class AudioManager : MonoBehaviour
{

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public float delay;

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

    public float getCurrentPoint()
    {
        return mainTheme.time; 
    }


    public void StartTheme()
    {
        mainTheme = GetComponent<AudioSource>();
        //mainTheme.Play();
    }
}

/* 
 * 8 seconds - first transition; .25 seconds per beat 
 * 30 seconds - second transition; 
 */ 