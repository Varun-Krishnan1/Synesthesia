using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RetryCannon : ActionItem
{

    public override void DoAction()
    {
        // -- Reset Current Progress 
        GameManager.Instance.RetrySecondStage(); 
    }
}
