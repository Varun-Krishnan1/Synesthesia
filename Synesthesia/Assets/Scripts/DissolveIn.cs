using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveIn : MonoBehaviour
{
    public float lerpDuration;
    public bool sharedMaterial; 

    private float startValue = .83f;
    private float endValue = 0f;
    private float valueToLerp;

    private Material shader;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Lerp()); 
    }

    // Update is called once per frame
    IEnumerator Lerp()
    {
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;

            if(sharedMaterial)
            {
                this.gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("AlphaClip", valueToLerp);
            }
            else
            {
                this.gameObject.GetComponent<Renderer>().material.SetFloat("AlphaClip", valueToLerp);
            }

            yield return null;
        }

        // snap to end value at finish
        if (sharedMaterial)
        {
            this.gameObject.GetComponent<Renderer>().sharedMaterial.SetFloat("AlphaClip", endValue);
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().material.SetFloat("AlphaClip", endValue);
        }
    }
}
