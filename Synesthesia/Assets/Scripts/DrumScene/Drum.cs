using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;


public class Drum : MonoBehaviour
{
    public drumTypes drumType;
    public bool playOnButtonPress = false;
    public ActionBasedController left_controller; 
    public ActionBasedController right_controller;
    public float saturationChangeOnCorrectHit; 

    private float previousRightTriggerValue; 
    private float previousLeftTriggerValue; 

    private float scaleHeight = .01f;
    private float scaleTime = .10f;

    private AudioSource source;
    private bool drumIsActive;
    private bool scaleInProgress = false; 

    private float originalH, originalS, originalV; 

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        drumIsActive = true;

        Color.RGBToHSV(transform.parent.GetComponentInChildren<Renderer>().material.GetColor("Base_Color"), out originalH, out originalS, out originalV);
    }

    private void OnTriggerEnter(Collider other)
    {


        DrumStick drumstick = other.GetComponent<DrumStick>(); 
        if (drumstick != null)
        {
            //Debug.Log("this y: " + transform.position.y);
            //Debug.Log("drumstick y: " + drumstick.transform.position.y);
            
            if(drumType == drumTypes.Kick)
            {
                // MUSIC 
                ActivateSound(other.GetComponent<DrumStick>().GetSpeed());

                // VISUAL 
                ScaleDrum();
                RequestVisualChange();


                // VIBRATION 
                // normalize between 0 and 1 assuming max un-normalized value is 25 
                float vibrationIntensity = other.GetComponent<DrumStick>().GetSpeed() / 25;
                drumstick.SendHapticImpulse(vibrationIntensity, .25f);
            }
            // make sure drumstick hitting from above only for all drums besides kick
            else if ((other.transform.position.y > this.transform.position.y))
            {
                // MUSIC 
                ActivateSound(other.GetComponent<DrumStick>().GetSpeed());

                // VISUAL 
                ScaleDrum();
                RequestVisualChange(); 


                // VIBRATION 
                // normalize between 0 and 1 assuming max un-normalized value is 25 
                float vibrationIntensity = other.GetComponent<DrumStick>().GetSpeed() / 25;
                drumstick.SendHapticImpulse(vibrationIntensity, .25f); 

            }
        }

    }

    void ScaleDrum()
    {
        if(!scaleInProgress)
        {
            scaleInProgress = true; 
            transform.parent.DOScale(transform.parent.localScale + new Vector3(scaleHeight, scaleHeight, scaleHeight), scaleTime).SetLoops(2, LoopType.Yoyo).OnComplete(() => { scaleInProgress = false; });
        }

    }

    public IEnumerator CorrectHitEffect(float animationTime)
    {
        Debug.Log(drumType + " Effect");

        float H, S, V;
        Color.RGBToHSV(transform.parent.GetComponentInChildren<Renderer>().material.GetColor("Base_Color"), out H, out S, out V);
        
        SetDrumColor(H, S - saturationChangeOnCorrectHit, V); 

        yield return new WaitForSeconds(animationTime);

        SetDrumColor(originalH, originalS, originalV); 
    }
    
    
    void SetDrumColor(float H, float S, float V)
    {
        foreach (Renderer r in transform.parent.GetComponentsInChildren<Renderer>())
        {
            if (r != this.GetComponent<Renderer>())
            {
                foreach (Material m in r.materials)
                {
                    m.SetColor("Base_Color", Color.HSVToRGB(H, S, V));
                }
            }
        }

    }
    void RequestVisualChange()
    {
        if (drumIsActive)
        {
            VisualManager.Instance.RequestElementChange(this);
        }
    }


    void Update()
    {
        if(playOnButtonPress)
        {
            CheckButtonPress(); 
        }


        drumIsActive = VisualManager.Instance.GetIsActive(drumType); 
    }

    public enum drumTypes
    {
        Snare, 
        HiHat, 
        FloorTom, 
        Kick, 
        HighTom,
        MidTom

    }

    private void ActivateSound(float volume)
    {
        source.pitch = Random.Range(0.95f, 1.05f);
        source.volume = volume; 
        source.PlayOneShot(source.clip, source.volume);

    }
    void CheckButtonPress()
    {
        float right_trigger_value = right_controller.activateAction.action.ReadValue<float>(); 
        float left_trigger_value = left_controller.activateAction.action.ReadValue<float>();

        bool buttonPressed = false; 
        if(previousRightTriggerValue < 0.5f && right_trigger_value > 0.5f)
        {
            ActivateSound(right_trigger_value);
            buttonPressed = true; 
        }
        else if(previousLeftTriggerValue < 0.5f && left_trigger_value > 0.5f)
        {
            ActivateSound(left_trigger_value);
            buttonPressed = true;
        }
        if(buttonPressed)
        {
            ScaleDrum();
            RequestVisualChange();

            // keep vibration intensity low 
            left_controller.SendHapticImpulse(.1f, .25f);
            right_controller.SendHapticImpulse(.1f, .25f);
        }

        previousRightTriggerValue = right_trigger_value;
        previousLeftTriggerValue = left_trigger_value;
    }
}
