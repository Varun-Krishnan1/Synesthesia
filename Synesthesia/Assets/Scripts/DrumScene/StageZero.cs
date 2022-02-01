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

    [Header("Stage 0 Locations")]
    public Transform blueLocation;
    public Transform greenLocation;
    public Transform orangeLocation;
    public Transform redLocation;
    public Transform pinkLocation;
    public Transform yellowLocation;

    public int[] levelProgressions;

    [Header("Stage 0 Objects")]
    public GameObject drumset; 
    public GameObject drumsticks;
    public Slider progressBar;
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

        progressBar.maxValue = levelProgressions[0];

        turnAroundObjects.SetActive(true);
        drumset.SetActive(false); 

        // -- ensure other objects are hidden 
        // progressBar.transform.parent.gameObject.SetActive(false);
        // drumsticks.SetActive(false);

        AudioManager.Instance.StartStageTheme(0);
    }

    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(timingOne);

        firstImage.SetActive(true);

        yield return new WaitForSeconds(timingTwo);

        firstImage.SetActive(false);

        yield return new WaitForSeconds(timingTwoPause);

        secondImage.SetActive(true);
        letters.SetActive(true); 

        yield return new WaitForSeconds(timingThree);

        secondImage.SetActive(false);
        letters.SetActive(false); 

        yield return new WaitForSeconds(timingThreePause);

        thirdImage.SetActive(true);
        fishSpawner.SetActive(true); 

        yield return new WaitForSeconds(timingFour);
        
        thirdImage.SetActive(false);
        Destroy(fishSpawner); // -- this destroys all fish as well since the fish are children
        
        GameManager.Instance.NextStage();
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
    public void DrumsticksGrabbed()
    {
        drumset.SetActive(true); 
        Destroy(turnAroundObjects);

        StartCoroutine(StartScene());

    }

    public void ProgressLevel(Gradient drumColor)
    {
        //if(endOfStage)
        //{
        //    return; 
        //}
        //curLevelProgression += 1;
        //progressBar.value = curLevelProgression; 
        //Debug.Log(curLevelProgression);

        //if(curLevelProgression == levelProgressions[GetLevel()])
        //{
        //    Debug.Log("Progressed!");
        //    // End of stage hit begin next stage 
        //    if(level+1 == levelProgressions.Length)
        //    {
        //        progressBar.transform.parent.gameObject.SetActive(false);
        //        endOfStage = true;
        //        GameManager.Instance.NextStage();
        //        return;
        //    }
        //    // Continue stage with text manager   
        //    else
        //    {
        //        TextManager.Instance.Activate();
        //    }
        //}
    }

}
