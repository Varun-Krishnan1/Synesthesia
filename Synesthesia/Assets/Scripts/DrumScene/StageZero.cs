using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
  
    private Dictionary<Drum.drumTypes, Transform> drumTypeToLocationDict = new Dictionary<Drum.drumTypes, Transform>();

    private int level = -1; // which portion of the stage you are on (make sure to start at -1) 
    private int curLevelProgression; 

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
    }

    public int GetLevel()
    {
        return level; 
    }

    public void NextLevel()
    {
        level += 1;
        curLevelProgression = 0;

        if (level == levelProgressions.Length)
        {
            GameManager.Instance.NextStage();
            return;
        }
    }

    public void ProgressLevel(Gradient drumColor)
    {
        curLevelProgression += 1;
        Debug.Log(curLevelProgression);

        if(curLevelProgression == levelProgressions[GetLevel()])
        {
            Debug.Log("Progressed!");
            TextManager.Instance.Activate();
        }
    }

}
