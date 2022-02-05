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

    public Ship userShip;
    public GameObject enemyShip;
    public GameObject sunkenShip; 
    public GameObject oldWater; 
    public GameObject newWater;
    public GameObject retryObject;
    public XRRig lossRig;
    public XRRig teleportationRig;

    public bool isUnderwater;
    [Header("Lose Scenario")]
    public float sinkDepth;
    public float sinkTime;
    public float drumstickDetachmentTime;
    [Header("Win Scenario")]
    public GameObject keyDrawings; 
    public int numKeysCollected;

    private Camera camera;
    private HandController left_controller;
    private HandController right_controller;
    private UnderwaterEffect underwaterEffect;


    void Awake() 
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        // -- disable current rig 
        GameObject.FindObjectOfType<XRRig>().gameObject.SetActive(false); 

        // -- set active the correct new rig 
        if (GameManager.Instance.stageThreeWin)
        {
            Destroy(lossRig.gameObject);
            teleportationRig.gameObject.SetActive(true); 
        }
        else
        {
            lossRig.gameObject.SetActive(true);
            Destroy(teleportationRig.gameObject); 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.stageThreeWin)
        {
            StartWinScene(); 
        }
        else
        {
            StartCoroutine(StartLoseScene());
        }
    }

    public void CollectKey(int keyNumber)
    {
        numKeysCollected += 1;

        keyDrawings.transform.GetChild(keyNumber).gameObject.SetActive(true); 
    }


    void StartWinScene()
    {
        enemyShip.gameObject.SetActive(false);
    }


    IEnumerator StartLoseScene()
    {
        XRRig rig = lossRig;
        camera = rig.gameObject.GetComponentsInChildren<Camera>()[0];
        underwaterEffect = camera.GetComponent<UnderwaterEffect>();
        HandController[] controllers = rig.gameObject.GetComponentsInChildren<HandController>();
        left_controller = controllers[0];
        right_controller = controllers[1];

        yield return new WaitForSeconds(0f); 

        camera.GetComponent<UnderwaterEffect>().enabled = true; 
        rig.transform.parent = userShip.gameObject.transform;
        userShip.gameObject.transform.DOMoveY(userShip.gameObject.transform.position.y - sinkDepth, sinkTime);

        sunkenShip.SetActive(false); 

        StartCoroutine(UnderwaterEffects());

        yield return new WaitForSeconds(sinkTime);

        retryObject.SetActive(true); 

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

        yield return new WaitForSeconds(drumstickDetachmentTime);

        //AudioManager.Instance.StartStageTheme(3);
        left_controller.EnableHands();
        right_controller.EnableHands();
    }
}
