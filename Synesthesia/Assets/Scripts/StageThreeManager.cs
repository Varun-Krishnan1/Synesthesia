using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StageThreeManager : MonoBehaviour
{
    public ActionBasedController[] controllers; 

    // Start is called before the first frame update
    void Start()
    {
        foreach(ActionBasedController controller in controllers)
        {
            DrumStick drumstick = controller.gameObject.GetComponentsInChildren<DrumStick>()[0];
            drumstick.transform.parent.gameObject.transform.parent = null; // -- unparent from controller 
            // -- Add hands back onto controller (shouldn't destroy them in first place just set them inactive in Stage 0) 
            // -- TODO: Add box collider and rigid body to drumstick 

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
