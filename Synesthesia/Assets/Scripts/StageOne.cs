using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOne : MonoBehaviour
{
    private static StageOne _instance;

    public static StageOne Instance { get { return _instance; } }

    public GameObject[] shipParts; 
    public float spawnDelay;
    public bool colorCloudsOnHit = false; 

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
        yield return new WaitForSeconds(spawnDelay);

        // -- for testing let them do color clouds
        colorCloudsOnHit = true; 
        // ---------------------------------------

        foreach (GameObject s in shipParts)
        {
            s.SetActive(true);
            yield return new WaitForSeconds(s.GetComponent<DissolveIn>().lerpDuration); 
        }

    }
}
