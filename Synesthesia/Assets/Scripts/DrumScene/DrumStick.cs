using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrumStick : MonoBehaviour
{
    private Vector3 lastPosition;
    private float speed;
    private bool attaching = false;
    private float waitFor = 0f; 

    private ActionBasedController controller; 
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // -- have to manually track speed since is kinematic rigidbody 
        speed = (transform.position - lastPosition).magnitude / Time.fixedDeltaTime;
        lastPosition = transform.position; 
    }

    void Update()
    {
        if(!controller)
        {
            ActionBasedController parentController = GetComponentInParent<ActionBasedController>();
            if(parentController)
            {
                controller = parentController; 
            }
        }

        if (attaching)
        {
            Debug.Log(waitFor);
            waitFor -= Time.deltaTime;
            if (waitFor <= 0f)
            {
                Debug.Log("Attached!");
                this.transform.parent = controller.transform;
                attaching = false;
            }
        }
    }

    public float GetSpeed()
    {
        return speed; 
    }

    public void SendHapticImpulse(float intensity, float duration)
    {
        controller.SendHapticImpulse(intensity, duration);
    }


    public void AttachToParent()
    {
        Debug.Log("AttachToParent()");
        waitFor = 0.01f;
        attaching = true;
    }
}
