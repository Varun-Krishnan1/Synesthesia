using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public GameObject chestLid;
    public Light pointLight; 
    public void CheckKeys()
    {
        if(StageThree.Instance.numKeysCollected == 3)
        {
            chestLid.GetComponent<Animator>().enabled = true;
            pointLight.gameObject.GetComponent<Animator>().enabled = true; 
        }
    }
}
