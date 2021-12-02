using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageZero : MonoBehaviour
{
    private static StageZero _instance;

    public static StageZero Instance { get { return _instance; } }

    [Header("Stage 0 Locations")]
    public Transform blueLocation;
    public Transform greenLocation;
    public Transform orangeLocation;
    public Transform redLocation;
    public Transform pinkLocation;
    public Transform yellowLocation;

    public int[] levelProgressions;

    [Header("Stage 0 Objects")]
    public GameObject drumsticks;
    public Slider progressBar;

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

        // -- ensure objects are hidden 
        progressBar.transform.parent.gameObject.SetActive(false);
        drumsticks.SetActive(false); 
    }

    public int GetLevel()
    {
        return level; 
    }

    public void NextLevel()
    {
        level += 1;

        // this level of the stage you grab the drumsticks 
        if (level == 0)
        {
            TextManager.Instance.WriteText("Grab the drumsticks in front of you to try it out!");
            progressBar.transform.parent.gameObject.SetActive(true);
            drumsticks.SetActive(true);
        }
        else
        {
            curLevelProgression = 0;
            progressBar.value = 0;
            progressBar.maxValue = levelProgressions[level]; 
        }
    }

    public void ProgressLevel(Gradient drumColor)
    {
        if(endOfStage)
        {
            return; 
        }
        curLevelProgression += 1;
        progressBar.value = curLevelProgression; 
        Debug.Log(curLevelProgression);

        if(curLevelProgression == levelProgressions[GetLevel()])
        {
            Debug.Log("Progressed!");
            // End of stage hit begin next stage 
            if(level+1 == levelProgressions.Length)
            {
                progressBar.transform.parent.gameObject.SetActive(false);
                endOfStage = true;
                GameManager.Instance.NextStage();
                return;
            }
            // Continue stage with text manager   
            else
            {
                TextManager.Instance.Activate();
            }
        }
    }

}
