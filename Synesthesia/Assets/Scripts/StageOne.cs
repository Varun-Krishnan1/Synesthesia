using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using LowPolyUnderwaterPack; 

public class StageOne : MonoBehaviour
{
    private static StageOne _instance;

    public static StageOne Instance { get { return _instance; } }

    [Header("Objects")]
    public Ship ship; 
    public GameObject[] shipParts;
    public GameObject wheel; 
    public GameObject water;
    public GameObject waterTwo; 
    public WaterMesh waterMesh; 
    public GameObject terrain;
    public GameObject blueDrum;
    public GameObject pinkDrum;

    [Header("General")]
    public float spawnDelay;
    public bool colorCloudsOnHit = false;
    public float drumCorrectHitEffectAnimationTime; 

    [Header("Boat Movement")]
    public float boatSlowdownInterval;
    public float maxBoatSpeed; 
    public float boatIncreaseSpeedConstant;
    public float boatIncreaseSpeedScaleFactor;
    public float boatDecreaseSpeedScaleFactor;
    public float boatDecreaseSpeedConstantFactor;
    public float noiseSpeedFactor;
    public float waveSpeed1Factor;
    public float waveSpeed2Factor;
    public float boatTargetSpeed;
    public float slowSpeedInterval;


    private bool moveShip = false;
    public float moveSpeed = 0f;
    private bool firstDrum = false;
    private bool beatVisualizer = false;
    private bool approachingEnemy = false;
    private float slowTime;

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

        AudioManager.Instance.StartStageTheme(1);

        StartCoroutine(SpawnShip()); 
    }

    IEnumerator SpawnShip()
    {
        // -- set parent ship container to active 
        shipParts[0].transform.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(spawnDelay);


        // -- for testing let them do color clouds
        // colorCloudsOnHit = true;
        // ---------------------------------------

        foreach (GameObject s in shipParts)
        {
            s.SetActive(true);
            yield return new WaitForSeconds(s.GetComponent<DissolveIn>().lerpDuration);
        }

        SpawnWaterAndTerrain();

        yield return new WaitForSeconds(10f);

        TextManager.Instance.Activate();


        yield return new WaitForSeconds(0f); 

        beatVisualizer = true;
        BeatManager.Instance.Activate();
        BeatManager.Instance.speed = 0.8f; 
    }

    void SpawnWaterAndTerrain()
    {
        water.SetActive(true);
        waterTwo.SetActive(true); 
        terrain.SetActive(true); 
    }

    public void DrumShipMovement(Drum drum)
    {
        Debug.Log("DrumShipMovement");
        Drum.drumTypes drumType = drum.drumType; 

        if(beatVisualizer)
        {
            if (drum.CorrectHit())
            {
                Debug.Log("Correct!");
                if (!approachingEnemy)
                {
                    if (moveSpeed == 0)
                    {
                        MoveShip(boatIncreaseSpeedConstant);
                    }
                    else
                    {
                        MoveShip(moveSpeed + (boatIncreaseSpeedScaleFactor * Mathf.Log(moveSpeed)));
                    }
                }
                else
                {
                    // -- TODO 
                }

                // -- drum scaling 
                StartCoroutine(drum.CorrectHitEffect(drumCorrectHitEffectAnimationTime));
            }
            else
            {
                Debug.Log("Incorrect!");
            }
        }
    }

    void MoveShip(float speed)
    {
        moveShip = true;
        moveSpeed = Mathf.Clamp(speed, 0f, maxBoatSpeed);

        Debug.Log("Move Speed set to: " + moveSpeed);
    }

    void Update()
    {
        if(moveShip)
        {
            waterMesh.noiseSpeed = moveSpeed / noiseSpeedFactor;
            waterMesh.waveSpeed1 = moveSpeed / waveSpeed1Factor;
            waterMesh.waveSpeed2 = moveSpeed / waveSpeed2Factor;
            terrain.transform.position -= new Vector3(0, 0, Time.deltaTime * moveSpeed * 2);
            
            //ship.transform.position += new Vector3(0, 0, Time.deltaTime * moveSpeed * 2);
            //water.transform.position += new Vector3(0, 0, Time.deltaTime * moveSpeed * 2);
            
        }

        // -- end of stage 1 
        if(approachingEnemy && moveSpeed == 0)
        {
            Debug.Log("Stage One Over!");
            StartCoroutine(NextStage());
        }
    }

    IEnumerator NextStage()
    {
        yield return new WaitForSeconds(3f);
        GameManager.Instance.NextStage();
    }

    void FixedUpdate()
    {
        if (beatVisualizer)
        {
            if(approachingEnemy)
            {
                if ((Time.time - slowTime) > slowSpeedInterval)
                {
                    MoveShip((moveSpeed * boatDecreaseSpeedScaleFactor) - boatDecreaseSpeedConstantFactor);
                    slowSpeedInterval += slowSpeedInterval; // slow down every 3 seconds from when hit slow checkpoint 
                }
            }

            // -- slow down boat speed every 3 seconds (300f) 
            if ((Mathf.RoundToInt(Time.time * 100f) % boatSlowdownInterval) == 0)
            {
                if(!approachingEnemy)
                {
                    MoveShip((moveSpeed * boatDecreaseSpeedScaleFactor) - boatDecreaseSpeedConstantFactor);
                }

                // -- wheel animation every 3 seconds as long as boat is not stationary 
                if (moveSpeed != 0)
                {
                    //wheel.transform.DORotate(new Vector3(0, 0, wheel.transform.rotation.eulerAngles.z + 180f), 3f);
                    wheel.transform.DORotate(new Vector3(0, 0, Random.Range(0f, 360f)), 3f);
                }
            }
        }
    }

    // called by ShipCheckpoint class 
    public void SlowBoatCheckpoint()
    {
        // -- give boat speed required to come to stop after certain distance 
        MoveShip(boatTargetSpeed);

        ship.Shake(); 
        slowTime = Time.time; 
        approachingEnemy = true;
        BeatManager.Instance.speed = BeatManager.Instance.speed * 2f; 
    }

    // -- to ensure it doesn't overshoot target 
    public void StopBoatCheckpoint()
    {
        MoveShip(0f);
        ship.Shake(); 
    }


}
