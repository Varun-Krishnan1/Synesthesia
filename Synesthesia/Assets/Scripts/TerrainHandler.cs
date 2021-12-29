using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainHandler : MonoBehaviour
{
    public float pacesToDestroy;

    private float originalZ; 
    // Start is called before the first frame update
    void Start()
    {
        originalZ = transform.position.z; 
    }

    // Update is called once per frame
    void Update()
    {
        // -- Every [pacesToDestroy] paces get rid of the terrain that went by and add new one in 
        float difference = Mathf.Abs(originalZ - transform.position.z);
        if (difference > pacesToDestroy)
        {
            Destroy(transform.GetChild(0).gameObject);

            // -- add in new one 
            foreach(Transform child in GetComponentsInChildren<Transform>(true))
            {
                if(!child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(true);
                    break; 
                }
            }
            pacesToDestroy += pacesToDestroy; 
        }
    }
}
