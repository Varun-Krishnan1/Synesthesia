using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio; 

public class AudioManager : MonoBehaviour
{

    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    public float delayTillMainTheme; 

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

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time; 
    }

    public float getCurrentTimePoint()
    {
        if(themeStartTime == 0f)
        {
            return 0f; 
        }
        return Time.time - themeStartTime; 
    }
    // Update is called once per frame
    void Update()
    {
        if((Time.time - startTime > delayTillMainTheme) && themeStartTime == 0)
        {
            AudioSource mainTheme = GetComponent<AudioSource>();
            mainTheme.Play();
            themeStartTime = Time.time;
        }
        // Debug.Log(Time.time - themeStartTime); 
    }
}

/* 
 * 8 seconds - first transition; .25 seconds per beat 
 * 30 seconds - second transition; 
 */ 