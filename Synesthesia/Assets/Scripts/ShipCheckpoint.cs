using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCheckpoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        Debug.Log(other.gameObject.GetComponent<ShipCollider>());
        if (other.gameObject.GetComponent<ShipCollider>())
        {
            StageOne.Instance.CheckpointHit(); 
        }
    }
}
