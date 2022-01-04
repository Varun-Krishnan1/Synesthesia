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

    public Drum[] drums; 
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


    public void RequestElementChange(Drum drum)
    {
        Debug.Log("Request Change"); 
        Drum.drumTypes drumType = drum.drumType; 

        if(drumType == Drum.drumTypes.Kick)
        {
            lastKickHit = .01f; 
        }

        int gameStage = GameManager.Instance.GetGameStage(); 
        if(gameStage == 0)
        {
            StageZero.Instance.ProgressLevel(drumTypeToColor(drumType)); 
            DrawColorSplash(StageZero.Instance.drumTypeToLocation(drumType), drumType); 
        }  
        if(gameStage == 1)
        {
            StageOne.Instance.DrumShipMovement(drum);
            // DrawColorSplash(StageZero.Instance.drumTypeToLocation(drumType), drumType);
        }
        if(gameStage == 2)
        {
            StageTwo.Instance.DrumHit(drum);
        }
    }

    public Gradient drumTypeToColor(Drum.drumTypes drumType)
    {
        return drumTypeToColorDict[drumType]; 
    }
    

    public GameObject DrawColorSplash(Vector3 position, Quaternion rotation, Drum.drumTypes drumType)
    {
        GameObject vfx = Instantiate(colorBurstVFX, position, rotation);
        VisualEffect vfx_effects = vfx.GetComponent<VisualEffect>();
        vfx_effects.SetVector4("Color", drumTypeToColor(drumType).colorKeys[0].color);

        return vfx; 
    }

    public GameObject DrawColorSplash(Transform location, Drum.drumTypes drumType)
    {
        return DrawColorSplash(location.position, location.rotation, drumType);
    }

    public GameObject DrawColorSplash(Vector3 position, Quaternion rotation, Vector3 scale, Drum.drumTypes drumType)
    {
        GameObject vfx = DrawColorSplash(position, rotation, drumType);
        vfx.transform.localScale = scale;

        return vfx; 
    }

}
