using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : ActionItem
{
    public int keyNumber;

    private bool activated; 

    public override void DoAction()
    {
        if(!activated)
        {
            activated = true; 

            StageThree.Instance.CollectKey(keyNumber, this.GetComponent<Renderer>().material.GetColor("BaseColor"));

            StartCoroutine(Dissolve());
        }

    }

    IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(2f);

        DissolveIn dissolveComponent = this.transform.GetComponent<DissolveIn>();
        dissolveComponent.Dissolve();

        yield return new WaitForSeconds(dissolveComponent.lerpDuration);

        Destroy(this.gameObject); 
    }

}
