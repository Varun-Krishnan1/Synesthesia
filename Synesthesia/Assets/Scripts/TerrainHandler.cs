using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHandler : MonoBehaviour
{
    public float pacesToDestroy;
    public float pacesToSpawn; 

    private float originalZ; 
    // Start is called before the first frame update
    void Start()
    {
        originalZ = transform.position.z; 
    }

    // Update is called once per frame
    void Update()
    {
        float difference = Mathf.Abs(originalZ - transform.position.z);

        // -- Every time you pass into new terrain generate the one after 
        if (difference > pacesToSpawn)
        {
            // -- add in new one 
            foreach (Transform child in GetComponentsInChildren<Transform>(true))
            {
                if (!child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(true);
                    break;
                }
            }
            pacesToSpawn += pacesToSpawn; 
        }
        // -- Every [pacesToDestroy] paces get rid of the terrain that went by 
        if (difference > pacesToDestroy)
        {
            Destroy(transform.GetChild(0).gameObject);
            pacesToDestroy += pacesToDestroy; 
        }
    }
}
