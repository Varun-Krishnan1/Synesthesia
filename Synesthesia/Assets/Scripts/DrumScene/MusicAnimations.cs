using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicAnimations : MonoBehaviour
{
    public AnimationCurve bobbingCurve;

    // -- bobbing variables 
    public float bobSpeed = 6.5f;
    public float bobHeight = 0.01f;

    Vector3 originalPosition; 

    // -- swaying variables 
    public float swaySpeed = 2.15f;
    public float swayHeight = 2f;
    public bool swayableItem = true; 

    Vector3 originalRotation; 

    private Vector3 pointA; 
    private Vector3 pointB;

    // Start is called before the first frame update
    void Start()
    {
        // -- rotation 
        pointA = transform.eulerAngles + new Vector3(0f, 0f, swayHeight);
        pointB = transform.eulerAngles + new Vector3(0f, 0f, -swayHeight);

        originalPosition = transform.position;
        originalRotation = transform.eulerAngles; 
    }

    // Update is called once per frame
    void Update()
    {
        if(AudioManager.Instance.getCurrentTimePoint() > 8 && AudioManager.Instance.getCurrentTimePoint() < 32)
        {
            Bob(); 
        }
        else if (AudioManager.Instance.getCurrentTimePoint() > 32 && AudioManager.Instance.getCurrentTimePoint() < 47)
        {
            transform.position = originalPosition; 
            Sway();
        }
        else if (AudioManager.Instance.getCurrentTimePoint() > 47 && AudioManager.Instance.getCurrentTimePoint() < 71)
        {
            transform.eulerAngles = originalRotation; 
        }
        else if (AudioManager.Instance.getCurrentTimePoint() > 78 && AudioManager.Instance.getCurrentTimePoint() < 101)
        {
            Bob(); 
        }
        else if (AudioManager.Instance.getCurrentTimePoint() > 101 && AudioManager.Instance.getCurrentTimePoint() < 116)
        {
            transform.position = originalPosition;
            Sway();
        }
        else if (AudioManager.Instance.getCurrentTimePoint() > 116)
        {
            transform.eulerAngles = originalRotation;
        }
    }

    void Bob()
    {
        
        Vector3 pos = transform.position;
        float newY = Mathf.Sin(Time.time * bobSpeed) * bobHeight + pos.y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void Sway()
    {
        if(swayableItem)
        {
            //PingPong between 0 and 1
            float time = Mathf.PingPong(Time.time * swaySpeed, 1);
            transform.eulerAngles = Vector3.Lerp(pointA, pointB, time);
        }

    }

}


