using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;

public class StageThreeManager : MonoBehaviour
{
    public GameObject ship; 
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
        ship.transform.DOMoveY(ship.transform.position.y - sinkDepth, sinkTime);

        yield return new WaitForSeconds(2f);

        left_controller.EnableHands();
        right_controller.EnableHands();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
