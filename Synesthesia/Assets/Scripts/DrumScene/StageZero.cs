using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageZero : MonoBehaviour
{
    private static StageZero _instance;

    public static StageZero Instance { get { return _instance; } }

    public bool testing = false; 

    [Header("Stage 0 and 4 Locations")]
    public Transform blueLocation;
    public Transform greenLocation;
    public Transform orangeLocation;
    public Transform redLocation;
    public Transform pinkLocation;
    public Transform yellowLocation;

    [Header("Stage 0 Objects")]
    public GameObject drumset; 
    public GameObject drumsticks;
    public GameObject turnAroundObjects;

    public GameObject firstImage;
    public GameObject secondImage;
    public GameObject thirdImage;

    public GameObject letters;
    public GameObject fishSpawner; 

    public float timingOne;
    public float timingTwo;
    public float timingTwoPause;
    public float timingThreePause;
    public float timingThree;
    public float timingFour;

    public int numDrumsticksPickedUp;

    [Header("Stage 4 Objects")]
    public GameObject firstCreditsImage;
    public GameObject secondCreditsImage;
    public GameObject thirdCreditsImage;
    public GameObject exitDrum; 

    public float creditsTimingOne;
    public float creditsTimingTwo;
    public float creditsTimingTwoPause;
    public float creditsTimingThreePause;
    public float creditsTimingThree;
    public float creditsTimingFour;

    private Dictionary<Drum.drumTypes, Transform> drumTypeToLocationDict = new Dictionary<Drum.drumTypes, Transform>();

    private int level = -1; // which portion of the stage you are on (make sure to start at -1) 
    private int curLevelProgression;
    private bool endOfStage = false; 

    public Transform drumTypeToLocation(Drum.drumTypes drumType)
    {
        return drumTypeToLocationDict[drumType];
    }

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
        drumTypeToLocationDict.Add(Drum.drumTypes.Snare, greenLocation);
        drumTypeToLocationDict.Add(Drum.drumTypes.FloorTom, orangeLocation);
        drumTypeToLocationDict.Add(Drum.drumTypes.MidTom, pinkLocation);
        drumTypeToLocationDict.Add(Drum.drumTypes.HighTom, blueLocation);
        drumTypeToLocationDict.Add(Drum.drumTypes.Kick, redLocation);
        drumTypeToLocationDict.Add(Drum.drumTypes.HiHat, yellowLocation);

        if(GameManager.Instance.gameStage == 0)
        {
            turnAroundObjects.SetActive(true);
            drumset.SetActive(false);
        }
        else if(GameManager.Instance.gameStage == 4)
        {
            // -- Reset position from treasure chest to back in front of drums
            GameManager.Instance.rig.transform.position = GameManager.Instance.stageTwoRigPosition;

            StartCoroutine(StartCreditsScene());
        }
    }

    // -- called by drumsticks grabbed function below 
    IEnumerator StartScene()
    {
        // -- don't play music till drumsticks are picked up 
        AudioManager.Instance.StartStageTheme(0);

        yield return new WaitForSeconds(timingOne);

        TextManager.Instance.DisplayImage(0, timingTwo);

        yield return new WaitForSeconds(timingTwo); 

        yield return new WaitForSeconds(timingTwoPause);

        TextManager.Instance.DisplayImage(1, timingThree);

        letters.SetActive(true);

        yield return new WaitForSeconds(timingThree);

        letters.SetActive(false);

        yield return new WaitForSeconds(timingThreePause);

        TextManager.Instance.DisplayImage(2, timingFour);

        fishSpawner.SetActive(true);
        AudioManager.Instance.PlaySoundEffect(2, .5f, .25f);

        yield return new WaitForSeconds(timingFour);

        Destroy(fishSpawner); // -- this destroys all fish as well since the fish are children

        GameManager.Instance.NextStage();
    }

    IEnumerator StartCreditsScene()
    {
        AudioManager.Instance.StartStageTheme(0);
        fishSpawner.SetActive(true);
        AudioManager.Instance.PlaySoundEffect(2, .5f, .25f); // -- fish splash

        yield return new WaitForSeconds(creditsTimingOne);

        TextManager.Instance.DisplayImage(3, creditsTimingTwo);

        yield return new WaitForSeconds(creditsTimingTwo); 
        yield return new WaitForSeconds(creditsTimingTwoPause);

        TextManager.Instance.DisplayImage(4, creditsTimingThree);

        yield return new WaitForSeconds(creditsTimingThree);
        yield return new WaitForSeconds(timingThreePause);

        TextManager.Instance.DisplayImage(5, creditsTimingFour);

        yield return new WaitForSeconds(creditsTimingFour);

        exitDrum.SetActive(true); 

    }


    public int GetLevel()
    {
        return level; 
    }

    void Update()
    {
        if(testing)
        {
            DrumsticksGrabbed();
            testing = false; 
        }
    }

    // -- called by hand controller when both drumsticks are picked up 
    public void DrumsticksGrabbed()
    {
        drumset.SetActive(true); 
        Destroy(turnAroundObjects);

        StartCoroutine(StartScene());

    }


}
