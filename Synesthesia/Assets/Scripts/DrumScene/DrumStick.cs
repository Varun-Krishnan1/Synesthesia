using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DrumStick : MonoBehaviour
{
    private Vector3 lastPosition;
    private float speed;

    private ActionBasedController controller; 
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        controller = GetComponentInParent<ActionBasedController>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // -- have to manually track speed since is kinematic rigidbody 
        speed = (transform.position - lastPosition).magnitude / Time.fixedDeltaTime;
        lastPosition = transform.position; 
    }

    public float GetSpeed()
    {
        return speed; 
    }

    public void SendHapticImpulse(float intensity, float duration)
    {
        controller.SendHapticImpulse(intensity, duration);
    }
}
