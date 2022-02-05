using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : ActionItem
{
    public int keyNumber; 

    public override void DoAction()
    {
        StageThree.Instance.CollectKey(keyNumber);
        this.transform.GetComponent<DissolveIn>().Dissolve(); 
    }
}
