using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VisualManager : MonoBehaviour
{

    private static VisualManager _instance;

    public static VisualManager Instance { get { return _instance; } }

    // -- storing order here for now but probably need to change it to other object 
    // -- or csv file in future 
    public float refractoryTime = 5f;
    private float lastKickHit;
    private bool haltPlaying = false; 

    public Transform[] drumObjectParticleFiringPoints; 
    public GameObject particleSystem;
    public GameObject testObject;

    [Header("Drum Colors")]
    public Gradient blue;      
    public Gradient green;
    public Gradient orange;
    public Gradient red;
    public Gradient pink;
    public Gradient yellow;

    public GameObject colorBurstVFX;
    
    
    private Dictionary<Drum.drumTypes, Gradient> drumTypeToColorDict = new Dictionary<Drum.drumTypes, Gradient>();


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

        drumTypeToColorDict.Add(Drum.drumTypes.Snare, green);
        drumTypeToColorDict.Add(Drum.drumTypes.FloorTom, orange);
        drumTypeToColorDict.Add(Drum.drumTypes.MidTom, pink);
        drumTypeToColorDict.Add(Drum.drumTypes.HighTom, blue);
        drumTypeToColorDict.Add(Drum.drumTypes.Kick, red);
        drumTypeToColorDict.Add(Drum.drumTypes.HiHat, yellow);

    }


    public void RequestElementChange(Drum.drumTypes drumType, GameObject DrumCollider)
    {        
        if(drumType == Drum.drumTypes.Kick)
        {
            lastKickHit = .01f; 
        }

        int gameStage = GameManager.Instance.GetGameStage(); 
        if(gameStage == 0)
        {
            Gradient drumColor = drumTypeToColor(drumType); 
            StageZero.Instance.ProgressLevel(drumColor); 
            DrawColorSplash(StageZero.Instance.drumTypeToLocation(drumType), drumColor); 
        }  
        if(gameStage == 1)
        {
            Gradient drumColor = drumTypeToColor(drumType);
            DrawColorSplash(StageZero.Instance.drumTypeToLocation(drumType), drumColor);
        }
    }

    public Gradient drumTypeToColor(Drum.drumTypes drumType)
    {
        return drumTypeToColorDict[drumType]; 
    }
    
    public void HaltPlaying()
    {
        this.haltPlaying = true; 
    }

    private bool SetDrumActive(Drum.drumTypes drumType)
    {
        // kick always needs a slight delay 
        if (drumType == Drum.drumTypes.Kick)
        {
            if (lastKickHit <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // if not kick then let them hit however many times they want 
        return true;
    }
    // -- change so its not in update but sends new active to next drum after RequestElementChange
    public bool GetIsActive(Drum.drumTypes drumType)
    {
        // Stage -1 is set when you shouldn't be able to play 
        if(haltPlaying)
        {
            return false; 
        }
        // Stage 0 drums always active 
        if(GameManager.Instance.GetGameStage() == 0)
        {
            return SetDrumActive(drumType); 
        }
        // Stage 1 drums only active when I say so 
        if(GameManager.Instance.GetGameStage() == 1 && StageOne.Instance.colorCloudsOnHit)
        {
            return SetDrumActive(drumType);
        }

        return false; 

    }

    void Update()
    {
        lastKickHit -= Time.deltaTime; 
    }

    void DrawColorSplash(Transform location, Gradient gradientColor)
    {
        GameObject vfx = Instantiate(colorBurstVFX, location.position, location.rotation);
        VisualEffect vfx_effects = vfx.GetComponent<VisualEffect>();
        vfx_effects.SetVector4("Color", gradientColor.colorKeys[0].color); 
    }
}
