using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RetryCannon : ActionItem
{

    public override void DoAction()
    {
        // -- Reset Current Progress 


        // Activate all rigs then destroy teleportation rig 
        // so the original one is active again 
        StageThree.Instance.originalRig.gameObject.SetActive(true); 

        // Destroy(StageThree.Instance.teleportationRig);

        GameManager.Instance.RetrySecondStage(); 
    }
}
