using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : ActionItem
{
    public int keyNumber; 

    public override void DoAction()
    {
        StageThree.Instance.CollectKey(keyNumber);

        StartCoroutine(Dissolve());
    }

    IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(2f); 

        this.transform.GetComponent<DissolveIn>().Dissolve();
    }
}
