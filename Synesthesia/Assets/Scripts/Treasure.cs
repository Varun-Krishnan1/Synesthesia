using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public ChestLid chestLid;
    public void CheckKeys()
    {
        if(StageThree.Instance.numKeysCollected == 3)
        {
            StageThree.Instance.treasureKeyDrawings.SetActive(false);
            chestLid.gameObject.GetComponent<Animator>().enabled = true;
        }
    }

    // Animation calls function on chest lid that calls function on this 
    public void OpeningAnimationFinished()
    {
        chestLid.gameObject.GetComponent<Animator>().enabled = false;

        // -- Win! 

    }
}
