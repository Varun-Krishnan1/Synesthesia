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

    public bool isUnderwater;
    [Header("Lose Scenario")]
    public float sinkDepth;
    public float sinkTime;
    public float drumstickDetachmentTime;
    [Header("Win Scenario")]
    public GameObject keyDrawings;
    public GameObject treasureKeyDrawings; 
    public int numKeysCollected;
    public GameObject left_controller;
    public GameObject right_controller;

    private Camera camera;

    private UnderwaterEffect underwaterEffect;
    private HandController lose_left_controller;
    private HandController lose_right_controller; 
    private GameObject rig; 

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

        rig = GameObject.FindObjectOfType<XRRig>().gameObject;
        HandController[] controllers = rig.gameObject.GetComponentsInChildren<HandController>();
        
        if (GameManager.Instance.stageThreeWin)
        {
            Destroy(controllers[0].gameObject);
            Destroy(controllers[1].gameObject);

            left_controller.transform.parent = rig.transform;
            right_controller.transform.parent = rig.transform;   
        }
        else
        {
            lose_left_controller = controllers[0];
            lose_right_controller = controllers[1]; 
            rig.transform.parent = userShip.gameObject.transform; 
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

    //// LOSE SCENE //// 
    
    IEnumerator StartLoseScene()
    {
        camera = rig.gameObject.GetComponentsInChildren<Camera>()[0];
        underwaterEffect = camera.GetComponent<UnderwaterEffect>();

        yield return new WaitForSeconds(0f); 

        camera.GetComponent<UnderwaterEffect>().enabled = true; 
        rig.transform.parent = userShip.gameObject.transform;
        userShip.gameObject.transform.DOMoveY(userShip.gameObject.transform.position.y - sinkDepth, sinkTime);

        sunkenShip.SetActive(false); 

        StartCoroutine(UnderwaterEffects());

        yield return new WaitForSeconds(sinkTime);

        retryObject.SetActive(true); 

    }

    IEnumerator UnderwaterEffects()
    {
        oldWater.SetActive(false);
        newWater.SetActive(true);

        yield return new WaitForSeconds(drumstickDetachmentTime);

        //AudioManager.Instance.StartStageTheme(3);
        lose_left_controller.EnableHands();
        lose_right_controller.EnableHands();
    }

    ///// WIN SCENE //// 
    
    public void CollectKey(int keyNumber, Color keyColor)
    {
        numKeysCollected += 1;

        GameObject key = keyDrawings.transform.GetChild(keyNumber).gameObject;
        key.GetComponent<Renderer>().material.SetColor("BaseColor", keyColor);
        key.GetComponentInChildren<Light>().enabled = true;

        GameObject treasureKey = treasureKeyDrawings.transform.GetChild(keyNumber).gameObject;
        treasureKey.GetComponent<Renderer>().material.SetColor("BaseColor", keyColor);
    }


    void StartWinScene()
    {
        enemyShip.gameObject.SetActive(false);
    }
}
