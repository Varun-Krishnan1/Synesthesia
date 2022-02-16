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
            AudioManager.Instance.PlaySoundEffect(17, 0.5f, .17f);

            StartCoroutine(Dissolve());
        }

    }

    IEnumerator Dissolve()
    {

        yield return new WaitForSeconds(2f);

        AudioManager.Instance.PlayDissolveSoundEffect(.5f, 0f);

        DissolveIn dissolveComponent = this.transform.GetComponent<DissolveIn>();
        dissolveComponent.Dissolve();

        yield return new WaitForSeconds(dissolveComponent.lerpDuration);

        Destroy(this.gameObject); 
    }

}
