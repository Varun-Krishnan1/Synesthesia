using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

public class StageThreeManager : MonoBehaviour
{
    public XRRig rig; 
    public HandController left_controller;
    public HandController right_controller;

    public float sinkDepth;
    public float sinkTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartScene());
    }

    IEnumerator StartScene()
    {
        rig.transform.DOMoveY(rig.transform.position.y - sinkDepth, sinkTime);

        yield return new WaitForSeconds(2f);

        left_controller.EnableHands();
        right_controller.EnableHands();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
