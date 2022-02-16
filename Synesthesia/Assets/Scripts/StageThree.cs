using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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
    public GameObject win_left_controller;
    public GameObject win_right_controller;
    public GameObject loss_left_controller;
    public GameObject loss_right_controller; 

    private Camera camera;

    private UnderwaterEffect underwaterEffect;
    private HandController orig_left_controller;
    private HandController orig_right_controller; 
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

        rig = GameManager.Instance.rig.gameObject;
        camera = rig.gameObject.GetComponentsInChildren<Camera>()[0];
        underwaterEffect = camera.GetComponent<UnderwaterEffect>();
        underwaterEffect.enabled = true; 

        HandController[] controllers = rig.gameObject.GetComponentsInChildren<HandController>();

        if (GameManager.Instance.stageThreeWin)
        {
            Destroy(controllers[0].gameObject);
            Destroy(controllers[1].gameObject); 

            win_left_controller.SetActive(true);
            win_right_controller.SetActive(true); 

            win_left_controller.transform.parent = rig.transform;
            win_right_controller.transform.parent = rig.transform;   
        }
        else
        {
            orig_left_controller = controllers[0];
            orig_right_controller = controllers[1];

            orig_left_controller.gameObject.SetActive(false);
            orig_right_controller.gameObject.SetActive(false);

            loss_left_controller.SetActive(true);
            loss_right_controller.SetActive(true); 

            loss_left_controller.transform.parent = rig.transform;
            loss_right_controller.transform.parent = rig.transform;  
        }

        AudioManager.Instance.ClearTheme(); 
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

    public void RetrySecondStage()
    {
        // -- Whoosh for grabbing cannon
        AudioManager.Instance.PlaySoundEffect(0, .5f, .12f);

        DOTween.Kill(GameManager.Instance.rig.gameObject); // -- remove tween on XRRig 

        loss_left_controller.GetComponent<HandController>().DestroySelfAndDrumstick(); 
        loss_right_controller.GetComponent<HandController>().DestroySelfAndDrumstick(); 

        orig_left_controller.gameObject.SetActive(true);
        orig_right_controller.gameObject.SetActive(true);

        underwaterEffect.isUnderwater = false;
        underwaterEffect.activated = false;
        underwaterEffect.enabled = false; 

        rig.gameObject.GetComponentsInChildren<Camera>()[0].GetComponent<Volume>().profile = null;

        GameManager.Instance.RetrySecondStage(); 
    }


    IEnumerator StartLoseScene()
    {
        yield return new WaitForSeconds(0f); 

        userShip.gameObject.transform.DOMoveY(userShip.gameObject.transform.position.y - sinkDepth, sinkTime);

        // -- don't change parent of rig to ship because it takes it out of DontDestroyOnLoad scene 
        rig.gameObject.transform.DOMoveY(rig.transform.position.y - sinkDepth, sinkTime);

        sunkenShip.SetActive(false); 

        StartCoroutine(UnderwaterEffects());

        yield return new WaitForSeconds(sinkTime);

        retryObject.SetActive(true); 

    }

    IEnumerator UnderwaterEffects()
    {
        AudioManager.Instance.StartStageTheme(3);

        oldWater.SetActive(false);
        newWater.SetActive(true);

        yield return new WaitForSeconds(drumstickDetachmentTime);

        loss_left_controller.GetComponent<HandController>().EnableHands();
        loss_right_controller.GetComponent<HandController>().EnableHands();

        // Orca Sound 
        yield return new WaitForSeconds(8f);
        AudioManager.Instance.PlaySoundEffect(9, .5f, .7f);


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

    public void EndingScene()
    {
        AudioManager.Instance.PlaySoundEffect(20, .5f, .65f);
    }
    void StartWinScene()
    {
        enemyShip.gameObject.SetActive(false);
    }
}
