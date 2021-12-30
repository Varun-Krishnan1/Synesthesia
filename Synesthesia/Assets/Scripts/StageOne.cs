using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class StageOne : MonoBehaviour
{
    private static StageOne _instance;

    public static StageOne Instance { get { return _instance; } }

    [Header("Objects")]
    public GameObject[] shipParts;
    public GameObject wheel; 
    public GameObject water;
    public Material waterMaterial; 
    public GameObject terrain;
    public GameObject blueDrum;
    public GameObject pinkDrum; 

    [Header("General")]
    public float spawnDelay;
    public bool colorCloudsOnHit = false;
    public float drumCorrectHitEffectAnimationTime; 

    [Header("Boat Movement")]
    public float beatTiming;
    public float beatLeeway;
    public float boatSlowdownInterval;
    public float boatIncreaseSpeedConstant;
    public float boatIncreaseSpeedScaleFactor;
    public float boatDecreaseSpeedScaleFactor;
    public float boatDecreaseSpeedConstantFactor;




    private bool moveShip = false;
    private float moveSpeed = 0f;
    private bool firstDrum = false;
    private bool beatVisualizer = false; 

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
        /*
        // -- set parent ship container to active 
        shipParts[0].transform.parent.gameObject.SetActive(true); 
        yield return new WaitForSeconds(spawnDelay);

        // -- for testing let them do color clouds
        colorCloudsOnHit = true;
        // ---------------------------------------

        foreach (GameObject s in shipParts)
        {
            s.SetActive(true);
            yield return new WaitForSeconds(s.GetComponent<DissolveIn>().lerpDuration); 
        }

        SpawnWaterAndTerrain();

        yield return new WaitForSeconds(5f);

        TextManager.Instance.Activate();
        */

        yield return new WaitForSeconds(0f); 

        blueDrum.SetActive(true);
        pinkDrum.SetActive(true);
        beatVisualizer = true; 


    }

    void SpawnWaterAndTerrain()
    {
        water.SetActive(true);
        terrain.SetActive(true); 
    }

    public void DrumShipMovement(Drum drum)
    {
        Drum.drumTypes drumType = drum.drumType; 

        if(beatVisualizer)
        {
            // .15 seconds leeway either direction 
            float timeOff = Mathf.RoundToInt(Time.time * 100f) % beatTiming;
            if ((timeOff < beatLeeway) || (timeOff > (beatTiming - beatLeeway)))
            {
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

                // -- color cloud over tutorial drums 
                Vector3 positionOffset = new Vector3(0f, .1f, 0f);
                Vector3 scale = new Vector3(.1f, .1f, .1f);
                VisualManager.Instance.DrawColorSplash(drumHit.transform.position + positionOffset, drumHit.transform.rotation, scale, drumType);

                // -- drum scaling 
                StartCoroutine(drum.CorrectHitEffect(drumCorrectHitEffectAnimationTime)); 

                if(moveSpeed == 0)
                {
                    MoveShip(boatIncreaseSpeedConstant); 
                }
                else
                {
                    MoveShip(moveSpeed + (boatIncreaseSpeedScaleFactor * Mathf.Log(moveSpeed)));
                }
            }
        }
    }

    void MoveShip(float speed)
    {
        moveShip = true;
        moveSpeed = Mathf.Clamp(speed, 0f, 50f);

        Debug.Log("Move Speed set to: " + moveSpeed);
    }

    void Update()
    {
        if(moveShip)
        {
            waterMaterial.SetFloat("Vector1_244B0600", moveSpeed);
            terrain.transform.position -= new Vector3(0, 0, Time.deltaTime * moveSpeed * 2);
        }
    }

    void FixedUpdate()
    {
        if (beatVisualizer)
        {
            // -- scale drum at interval of 1 second (100f) 
            float timeOff = Mathf.RoundToInt(Time.time * 100f) % beatTiming;

            // -- but change drum at interval a little before for leeway 
            if (timeOff == (beatTiming - beatLeeway))
            {
                firstDrum = !firstDrum;
            }


            if (timeOff == 0)
            {
                GameObject drum;
                if (firstDrum)
                {
                    drum = blueDrum;
                }
                else
                {
                    drum = pinkDrum;
                }

                float scaleFactor = .05f;
                drum.transform.DOScale(drum.transform.localScale + new Vector3(scaleFactor, 0, scaleFactor), .15f).SetLoops(2, LoopType.Yoyo);

                // -- slow down boat speed every 3 seconds (300f) 
                if((Mathf.RoundToInt(Time.time * 100f) % boatSlowdownInterval) == 0)
                {
                    MoveShip((moveSpeed * boatDecreaseSpeedScaleFactor) - boatDecreaseSpeedConstantFactor);

                    // -- wheel animation every 3 seconds as long as boat is not stationary 
                    if (moveSpeed != 0)
                    {
                        //wheel.transform.DORotate(new Vector3(0, 0, wheel.transform.rotation.eulerAngles.z + 180f), 3f);
                        wheel.transform.DORotate(new Vector3(0, 0, Random.Range(0f, 360f)), 3f);
                    }

                }
            }
        }
    }

    // called by ShipCheckpoint class 
    public void CheckpointHit()
    {
        MoveShip(0f);
        beatVisualizer = false;
        blueDrum.SetActive(false);
        pinkDrum.SetActive(false); 
    }
}
