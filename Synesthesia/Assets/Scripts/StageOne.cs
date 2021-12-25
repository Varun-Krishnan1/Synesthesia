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
    public GameObject terrain; 

    [Header("Variables")]
    public float spawnDelay;
    public bool colorCloudsOnHit = false;
    private bool moveTerrain = false;
    private float terrainMoveSpeed = 0f; 

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
        MoveTerrain(10f); 
        AudioManager.Instance.StartTheme();
        StartCoroutine(SpawnShip()); 
    }

    IEnumerator SpawnShip()
    {
        Debug.Log("HERE");

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
    }

    void SpawnWaterAndTerrain()
    {
        water.SetActive(true);
        terrain.SetActive(true); 
    }

    void MoveTerrain(float speed)
    {
        terrainMoveSpeed = speed;
        moveTerrain = true; 
    }

    void Update()
    {
        if(moveTerrain)
        {
            terrain.transform.position -= new Vector3(0, 0, Time.deltaTime * terrainMoveSpeed);
        }
    }
}
