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

            GameObject drumstick = this.GetComponent<XRDirectInteractor>().selectTarget.gameObject;
            drumstick.GetComponentInChildren<DrumStick>().controller = controller;

            // ensure both drumsticks are picked up
            if (StageZero.Instance.numDrumsticksPickedUp == 2)
            {
                kick.playOnButtonPress = true;
                // -- reset text back to normal after they grab 
                TextManager.Instance.ClearText();
            }

            hand.gameObject.SetActive(false);

            drumstickAttachedOnce = true;
        }
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

    public void EnableHands()
    {
        if(kick) kick.playOnButtonPress = false; 

        DrumStick drumstick = this.GetComponentsInChildren<DrumStick>()[0];
        drumstick.transform.parent.gameObject.transform.parent = null; // -- unparent from controller 
        
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
        if (this.GetComponent<XRDirectInteractor>().selectTarget)
        {
            // Check if it is an actionable object
            ActionItem actionItem = this.GetComponent<XRDirectInteractor>().selectTarget.gameObject.GetComponent<ActionItem>();
            if (actionItem)
            {
                actionItem.DoAction();
            }
        }
    }
}
