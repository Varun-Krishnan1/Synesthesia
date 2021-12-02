using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Drum : MonoBehaviour
{
    public drumTypes drumType;
    public bool playOnButtonPress = false;
    public ActionBasedController left_controller; 
    public ActionBasedController right_controller; 
    private float previousRightTriggerValue; 
    private float previousLeftTriggerValue; 

    private float scaleSpeed = 10f;
    private float scaleHeight = .01f;
    private float scaleTime = .15f;

    Vector3 pointA;
    Vector3 pointB;

    private float curScaleTime; 
    private AudioSource source;
    private bool drumIsActive;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        drumIsActive = true;

        // -- scaling 
        pointA = transform.parent.localScale;

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
        curScaleTime = scaleTime;

    }

    void RequestVisualChange()
    {
        if (drumIsActive)
        {
            VisualManager.Instance.RequestElementChange(drumType, this.gameObject);
        }
    }


    void Update()
    {
        if(playOnButtonPress)
        {
            CheckButtonPress(); 
        }


        // Scaling 
        pointB = pointA + new Vector3(scaleHeight, scaleHeight, scaleHeight);

        drumIsActive = VisualManager.Instance.GetIsActive(drumType); 
        if(curScaleTime >= 0f)
        {
            float time = Mathf.PingPong(Time.time * scaleSpeed, 1);
            transform.parent.localScale = Vector3.Lerp(pointA, pointB, time);
        }
        curScaleTime -= Time.deltaTime; 
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
        
        if((previousRightTriggerValue < 0.5f && right_trigger_value > 0.5f) || (previousLeftTriggerValue < 0.5f && left_trigger_value > 0.5f))
        {
            ActivateSound(right_trigger_value);
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
