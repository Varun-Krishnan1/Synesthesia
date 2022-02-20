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

    public bool drumstickDropped; 
    private bool isActive = true;

    public bool attachDrumstickBool; // -- FOR TESTING ONLY!! 
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

        if(attachDrumstickBool)
        {
            StageZero.Instance.numDrumsticksPickedUp += 1;

            if (StageZero.Instance.numDrumsticksPickedUp == 2)
            {
                kick.playOnButtonPress = true;
                StageZero.Instance.DrumsticksGrabbed();
            }
        }
    }

    IEnumerator AttachAfterDelay(DrumStick drumstick)
    {
        yield return new WaitForSeconds(.1f);
        drumstick.transform.parent.parent = this.transform;
        Destroy(drumstick.transform.parent.gameObject.GetComponent<XRGrabInteractable>());
        DisableInteractor(); 

    }
    public void AttachDrumstick(DrumStick drumstick)
    {
        AudioManager.Instance.PlaySoundEffect(0, 0.5f, .12f);

        drumstick.GetComponentInChildren<DrumStick>().controller = controller;

        // Lose stuff added to it by Interactor in SelectEntered
        Destroy(drumstick.transform.parent.gameObject.GetComponent<Rigidbody>());
        Destroy(drumstick.transform.parent.gameObject.GetComponent<BoxCollider>());

        // -- have to actually parent it after delay so it stays on grab target first 
        StartCoroutine(AttachAfterDelay(drumstick));

        hand.gameObject.SetActive(false);

        if (GameManager.Instance.gameStage == 0)
        {
            StageZero.Instance.numDrumsticksPickedUp += 1;

            // ensure both drumsticks are picked up
            if (StageZero.Instance.numDrumsticksPickedUp == 2)
            {
                kick.playOnButtonPress = true;
                StageZero.Instance.DrumsticksGrabbed();
            }
        }
        else if(GameManager.Instance.gameStage == 3)
        {
            //this.GetComponent<XRDirectInteractor>().enabled = false;
            //this.isActive = false;

            StageThree.Instance.numDrumsticksPickedUp += 1;
            // ensure both drumsticks are picked up
            if (StageThree.Instance.numDrumsticksPickedUp == 2)
            {
                StageThree.Instance.DrumsticksGrabbed();
            }
        }
    }

    public void DestroySelfAndDrumstick()
    {
        Destroy(drumstick);
        Destroy(this.gameObject);
    }

    public void DisableInteractor()
    {
        this.GetComponent<XRDirectInteractor>().enabled = false;
        this.isActive = false;
    }


    public void EnableHands()
    {
        if(kick) kick.playOnButtonPress = false; 

        drumstick.transform.parent = null; // -- unparent from controller 
        
        hand.gameObject.SetActive(true);
        this.GetComponent<XRDirectInteractor>().enabled = true;
        this.isActive = true;

        drumstickDropped = true; 
    }

    public void EnableHandsReward()
    {
        Destroy(drumstick); 

        hand.gameObject.SetActive(true);
        this.GetComponent<XRDirectInteractor>().enabled = true;
        this.isActive = true;
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

            DrumStick drumstick = selectTarget.gameObject.GetComponentInChildren<DrumStick>(); 
            if(drumstick)
            {
                AttachDrumstick(drumstick); 
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
