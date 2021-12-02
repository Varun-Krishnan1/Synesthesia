using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; 


[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{
    public Hand hand; 

    ActionBasedController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ActionBasedController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        hand.SetGrip(controller.selectAction.action.ReadValue<float>());
        hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
    }

    public void AttachDrumstick()
    {
        Destroy(hand.gameObject);

        GameObject drumstick = this.GetComponent<XRDirectInteractor>().selectTarget.gameObject;
        drumstick.transform.parent = this.transform; 
    }


    public void DestroyInteractor()
    {
        Destroy(this.GetComponent<XRDirectInteractor>());
        Destroy(this);
    }
}
