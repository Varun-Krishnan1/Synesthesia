using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{

    private static VisualManager _instance;

    public static VisualManager Instance { get { return _instance; } }

    // -- storing order here for now but probably need to change it to other object 
    // -- or csv file in future 
    public GameObject[] objectShowOrder;
    public Drum.drumTypes[] drumOrder;
    public float refractoryTime = 5f;
    private float lastHitTime; 

    public Transform[] drumObjectParticleFiringPoints; 
    public GameObject particleSystem;
    public GameObject testObject;

    [Header("Particle Gradients")]
    public Gradient blue;      
    public Gradient green;
    public Gradient brown;
    public Gradient red; 

    public Material materialToTransition; 

    private Dictionary<Drum.drumTypes, Gradient> drumTypeToColor = new Dictionary<Drum.drumTypes, Gradient>();
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



        foreach (GameObject g in objectShowOrder)
        {
            foreach(Transform child in g.GetComponentsInChildren<Transform>())
            {
                child.gameObject.SetActive(false); 
            }
            g.SetActive(false);
        }

        drumTypeToColor.Add(Drum.drumTypes.Blue, blue); 
        drumTypeToColor.Add(Drum.drumTypes.Green, green); 
        drumTypeToColor.Add(Drum.drumTypes.Brown, brown); 
        drumTypeToColor.Add(Drum.drumTypes.Red, red);

        lastHitTime = 0f; 
    }


    public void RequestElementChange(Drum.drumTypes drumType, GameObject DrumCollider)
    {
        //if(drumType == Drum.drumTypes.Brown)
        //{
        //    Debug.Log("Brown");
        //    GameObject mountain = MapManager.Instance.GetRandomDormantMountain();
        //    mountain.SetActive(true); 
        //}
        
        if(drumType == drumOrder[orderCounter])
        {
            // -- prevent other drums from firing 
            lastHitTime = refractoryTime; 

            Debug.Log(drumOrder[orderCounter]);
            objectShowOrder[orderCounter].SetActive(true);
            foreach (Transform child in objectShowOrder[orderCounter].transform)
            {
                GameObject ps = Instantiate(particleSystem, DrumCollider.transform.position, DrumCollider.transform.rotation);
                ps.AddComponent<ParticleSystemMovement>();
                ps.GetComponent<ParticleSystemMovement>().targetObject = child.gameObject;

                //var colLifetime = particleSystemMovement.gameObject.GetComponent<ParticleSystem>().colorOverLifetime;
                //colLifetime.color = drumTypeToColor[drumType];
                //particleSystemMovement.moveSpeed = 50f;

                //ps.GetComponent<particleAttractorLinear>().target = child.gameObject.transform; 
            }
            orderCounter = orderCounter + 1; 
        }
    }

    // -- change so its not in update but sends new active to next drum after RequestElementChange
    public bool GetIsActive(Drum.drumTypes drumType)
    {
        if (drumType == drumOrder[orderCounter] && lastHitTime <= 0)
        {
            return true; 
        }
        return false; 
    }

    void Update()
    {
        lastHitTime -= Time.deltaTime; 
    }
}
