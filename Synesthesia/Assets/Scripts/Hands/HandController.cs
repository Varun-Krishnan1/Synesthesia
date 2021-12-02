using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; 


[RequireComponent(typeof(ActionBasedController))]
public class HandController : MonoBehaviour
{
    public Hand hand;
    public Drum kick; 

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
        
        Destroy(hand.gameObject);

    }


    public void DestroyInteractor()
    {
        Destroy(this.GetComponent<XRDirectInteractor>());
        Destroy(this);
    }
}
