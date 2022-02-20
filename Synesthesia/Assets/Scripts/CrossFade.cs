using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossFade : MonoBehaviour
{
    public Animator transition;
    public float transitionAnimationTime;

    public void StartAnimation()
    {
        StartCoroutine(StartAnimationRoutine()); 
    }

    IEnumerator StartAnimationRoutine()
    {
        transition.SetBool("Start", true);

        yield return new WaitForSeconds(transitionAnimationTime);

        transition.SetBool("Start", false); 
    }
}
