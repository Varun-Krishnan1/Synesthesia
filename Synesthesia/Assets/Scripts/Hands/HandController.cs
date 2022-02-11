using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; 


[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{
    public Hand hand;
    public Drum kick;

    public bool enableHands; 

    public bool drumstickAttachedOnce;
    public bool interactorDestroyedOnce;
    public bool drumstickDropped; 
    private bool isActive = true;

    public GameObject drumstick; // -- manually set for stage 3 hand controller 

    ActionBasedController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<ActionBasedController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            hand.SetGrip(controller.selectAction.action.ReadValue<float>());
            hand.SetTrigger(controller.activateAction.action.ReadValue<float>());
        }

        if(enableHands)
        {
            EnableHands();
            enableHands = false; 
        }
    }

    public void AttachDrumstick()
    {
        if(!drumstickAttachedOnce)
        {
            StageZero.Instance.numDrumsticksPickedUp += 1;

            drumstick = this.GetComponent<XRDirectInteractor>().selectTarget.gameObject; // -- sets class variable too
            drumstick.GetComponentInChildren<DrumStick>().controller = controller;

            // ensure both drumsticks are picked up
            if (StageZero.Instance.numDrumsticksPickedUp == 2)
            {
                kick.playOnButtonPress = true;
                StageZero.Instance.DrumsticksGrabbed(); 
            }

            hand.gameObject.SetActive(false);

            drumstickAttachedOnce = true;
        }
    }

    public void DestroySelfAndDrumstick()
    {
        Destroy(drumstick);
        Destroy(this.gameObject);
    }

    public void DestroyInteractor()
    {
        if(!interactorDestroyedOnce)
        {
            this.GetComponent<XRDirectInteractor>().enabled = false;
            this.isActive = false;

            interactorDestroyedOnce = true;
        }
    }

    //public void ResetDrumsticks()
    //{
    //    drumstick.transform.parent = this.transform; // -- reparent to this controller 


    //    // -- reset variables so hands can be enabled again later 
    //    hand.gameObject.SetActive(false);
    //    this.GetComponent<XRDirectInteractor>().enabled = false;
    //    this.isActive = false; 

    //    drumstickDropped = false; 
    //}

    public void EnableHands()
    {
        if(kick) kick.playOnButtonPress = false; 

        drumstick.transform.parent = null; // -- unparent from controller 
        
        hand.gameObject.SetActive(true);
        this.GetComponent<XRDirectInteractor>().enabled = true;
        this.isActive = true;

        drumstickDropped = true; 
    }

    public void ToggleVisibility()
    {
        if(drumstickDropped)
        {
            hand.SetGrip(0f); 
            hand.SetTrigger(0f); 
            hand.gameObject.SetActive(!hand.gameObject.activeSelf);
        }

        // -- if picking something up 
        XRBaseInteractable selectTarget = this.GetComponent<XRDirectInteractor>().selectTarget; 
        if (selectTarget)
        {
            // Check if it is an actionable object
            ActionItem actionItem = selectTarget.gameObject.GetComponent<ActionItem>();
            if (actionItem)
            {
                actionItem.DoAction();
            }
            //else
            //{
            //    Rigidbody r = selectTarget.gameObject.GetComponent<Rigidbody>(); 
            //    if(r)
            //    {
            //        r.isKinematic = false; 
            //    }
            //}
        }
    }
}
