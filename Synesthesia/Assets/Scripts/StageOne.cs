using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOne : MonoBehaviour
{
    private static StageOne _instance;

    public static StageOne Instance { get { return _instance; } }

    [Header("Objects")]
    public GameObject[] shipParts;
    public GameObject water;
    public Material waterMaterial; 
    public GameObject terrain;
    public GameObject blueDrum;
    public GameObject pinkDrum; 
    public GameObject testObjectTwo; 

    [Header("Variables")]
    public float spawnDelay;
    public bool colorCloudsOnHit = false;
    public float speedDecayTime; 
    private bool moveShip = false;
    private float moveSpeed = 0f;
    private bool firstDrum = false; 

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

    void Start()
    {
        Debug.Log("Stage One Started!");

        AudioManager.Instance.StartTheme();
        StartCoroutine(SpawnShip()); 
    }

    IEnumerator SpawnShip()
    {
        Debug.Log("HERE");

        // -- set parent ship container to active 
        shipParts[0].transform.parent.gameObject.SetActive(true); 
        yield return new WaitForSeconds(spawnDelay);

        TextManager.Instance.Activate(); 
        // -- for testing let them do color clouds
        colorCloudsOnHit = true;
        // ---------------------------------------

        foreach (GameObject s in shipParts)
        {
            s.SetActive(true);
            yield return new WaitForSeconds(s.GetComponent<DissolveIn>().lerpDuration); 
        }

        SpawnWaterAndTerrain(); 
    }

    void SpawnWaterAndTerrain()
    {
        water.SetActive(true);
        terrain.SetActive(true); 
    }

    public void DrumShipMovement(Drum.drumTypes drumType)
    {
        if(AudioManager.Instance.getCurrentPoint() >= 0f)
        {
            // .15 seconds leeway either direction 
            float timeOff = Mathf.RoundToInt(Time.time * 100f) % 100f;
            Debug.Log(timeOff); 
            if ((timeOff < 15f) || (timeOff > 85f))
            {
                Vector3 positionOffset = new Vector3(0f, .1f, 0f);
                Vector3 scale = new Vector3(.1f, .1f, .1f);
                GameObject drumHit; 

                if(firstDrum && drumType == Drum.drumTypes.HighTom)
                {
                    drumHit = blueDrum; 
                }
                else if(!firstDrum && drumType == Drum.drumTypes.MidTom)
                {
                    drumHit = pinkDrum; 
                }
                else
                {
                    return; 
                }

                VisualManager.Instance.DrawColorSplash(drumHit.transform.position + positionOffset, drumHit.transform.rotation, scale, drumType);
                MoveShip(moveSpeed + 1f); 
            }
        }
    }

    void MoveShip(float speed)
    {
        moveSpeed = Mathf.Clamp(speed, 0f, 50f);
        moveShip = true;

        Debug.Log("Move Speed set to: " + moveSpeed);
    }

    void Update()
    {
        if(moveShip)
        {
            // TODO - FIX DECAYING 

            // speed decays over time 
            moveSpeed -= Time.deltaTime * speedDecayTime;
            moveSpeed = Mathf.Clamp(moveSpeed, 0f, 50f);

            Debug.Log(moveSpeed); 

            waterMaterial.SetFloat("Vector1_244B0600", moveSpeed);
            terrain.transform.position -= new Vector3(0, 0, Time.deltaTime * moveSpeed * 2);

        }
    }

    void FixedUpdate()
    {        
        // -- scale drum at interval of 1 second (100f) 
        float timeOff = Mathf.RoundToInt(Time.time * 100f) % 100f;

        // -- but change drum at interval a little before for leeway 
        if(timeOff == 85)
        {
            firstDrum = !firstDrum; 
        }


        if (timeOff == 0)
        {
            Vector3 localScale = blueDrum.transform.localScale;
            float scaleFactor = .04f;

            GameObject drum; 
            if(firstDrum)
            {
                drum = blueDrum; 
            }
            else
            {
                drum = pinkDrum;
            }

            iTween.ScaleFrom(drum, new Vector3(localScale.x + scaleFactor, localScale.y + scaleFactor, localScale.z + scaleFactor), .15f);
        }
    }
}
