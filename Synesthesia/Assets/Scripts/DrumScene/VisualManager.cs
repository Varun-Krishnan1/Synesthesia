using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{

    private static VisualManager _instance;

    public static VisualManager Instance { get { return _instance; } }

    // -- storing order here for now but probably need to change it to other object 
    // -- or csv file in future 
    public float refractoryTime = 5f;
    private float lastHitTime; 

    public Transform[] drumObjectParticleFiringPoints; 
    public GameObject particleSystem;
    public GameObject testObject;

    [Header("Drum Colors")]
    public Gradient blue;      
    public Gradient green;
    public Gradient brown;
    public Gradient red;

    [Header("Stage 0")]
    public Transform testTransform; 
    public GameObject colorSplash; 

    public Material materialToTransition; 

    private Dictionary<Drum.drumTypes, Gradient> drumTypeToColorDict = new Dictionary<Drum.drumTypes, Gradient>();
    private int orderCounter = 0; 

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

        // -- add transition to material script to all spawnable objects 
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("SpawnableObjectMesh")) {

            // Material transition animation 
            g.AddComponent<MaterialTransition>();
            g.GetComponent<MaterialTransition>().SetMaterial(materialToTransition);

            // Audio 
            g.AddComponent<MusicAnimations>();

            if (g.name.Contains("hill"))
            {
                g.GetComponent<MusicAnimations>().swayableItem = false;
            }

        }

        GameObject ground = GameObject.FindWithTag("Ground");
        ground.AddComponent<MusicAnimations>();
        ground.GetComponent<MusicAnimations>().swayableItem = false;


        drumTypeToColorDict.Add(Drum.drumTypes.Snare, brown);
        drumTypeToColorDict.Add(Drum.drumTypes.FloorTom, green);
        drumTypeToColorDict.Add(Drum.drumTypes.MidTom, red);
        drumTypeToColorDict.Add(Drum.drumTypes.HighTom, blue);

        lastHitTime = 0f; 
    }


    public void RequestElementChange(Drum.drumTypes drumType, GameObject DrumCollider)
    {        
        // -- prevent other drums from firing 
        lastHitTime = refractoryTime;

        int gameStage = GameManager.Instance.GetGameStage(); 
        if(gameStage == 0)
        {
            DrawColorSplash(drumTypeToLocation(drumType), drumTypeToColor(drumType)); 
        }
        //foreach (Transform child in objectShowOrder[orderCounter].transform)
        //{
        //    GameObject ps = Instantiate(particleSystem, DrumCollider.transform.position, DrumCollider.transform.rotation);
        //    ps.AddComponent<ParticleSystemMovement>();
        //    ps.GetComponent<ParticleSystemMovement>().targetObject = child.gameObject;

        //    //var colLifetime m  = particleSystemMovement.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
        //    //colLifetime.color = drumTypeToColor[drumType];
        //    //particleSystemMovement.moveSpeed = 50f;

        //    //ps.GetComponent<particleAttractorLinear>().target = child.gameObject.transform; 
        //}
        
    }
    
    private Transform drumTypeToLocation(Drum.drumTypes drumType)
    {
        return testTransform; 
    }

    private Gradient drumTypeToColor(Drum.drumTypes drumType)
    {
        return drumTypeToColorDict[drumType]; 
    }
    
    // -- change so its not in update but sends new active to next drum after RequestElementChange
    public bool GetIsActive(Drum.drumTypes drumType)
    {
        if (lastHitTime <= 0)
        {
            return true; 
        }
        return false; 
    }

    void Update()
    {
        lastHitTime -= Time.deltaTime; 
    }

    void DrawColorSplash(Transform location, Gradient color)
    {
        Instantiate(colorSplash, location.position, location.rotation);
        colorSplash.GetComponent<ColorSplash>().color = color; 
    }
}
