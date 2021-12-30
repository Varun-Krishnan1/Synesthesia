using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCheckpoint : MonoBehaviour
{
    public bool slowBoat;
    public bool stopBoat; 
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ShipCollider>())
        {
            if(slowBoat)
            {
                StageOne.Instance.SlowBoatCheckpoint();
            }
            if(stopBoat)
            {
                StageOne.Instance.StopBoatCheckpoint(); 
            }
        }
    }
}
