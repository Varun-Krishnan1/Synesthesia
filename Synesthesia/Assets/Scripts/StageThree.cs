using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;
using LowPolyUnderwaterPack; 

public class StageThree : MonoBehaviour
{
    private static StageThree _instance;

    public static StageThree Instance { get { return _instance; } }

    public GameObject ship;
    public GameObject oldWater; 
    public GameObject newWater; 

    private Camera camera; 
    private XRRig rig;
    private HandController left_controller;
    private HandController right_controller;
    private UnderwaterEffect underwaterEffect;

    public float sinkDepth;
    public float sinkTime;
    public float drumstickDetachmentTime; 

    public bool isUnderwater;


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.Instance.ClearTheme();

        rig = GameObject.FindObjectOfType<XRRig>();
        camera = rig.gameObject.GetComponentsInChildren<Camera>()[0];
        underwaterEffect = camera.GetComponent<UnderwaterEffect>(); 
        HandController[] controllers = rig.gameObject.GetComponentsInChildren<HandController>();
        left_controller = controllers[0];
        right_controller = controllers[1];

        StartCoroutine(StartScene()); 
    }

    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(0f); 

        camera.GetComponent<UnderwaterEffect>().enabled = true; 
        rig.transform.parent = ship.transform; 
        ship.transform.DOMoveY(ship.transform.position.y - sinkDepth, sinkTime);

        StartCoroutine(UnderwaterEffects());

    }

    // Update is called once per frame
    void Update()
    {

        //if (underwaterEffect.activated && !isUnderwater)
        //{
        //    StartCoroutine(UnderwaterEffects()); 
        //    isUnderwater = true; 
        //}
    }

    IEnumerator UnderwaterEffects()
    {
        oldWater.SetActive(false);
        newWater.SetActive(true);

        yield return new WaitForSeconds(5f);

        //AudioManager.Instance.StartStageTheme(3);
        left_controller.EnableHands();
        right_controller.EnableHands();
    }
}
