using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestLid : MonoBehaviour
{
    /// -- Sole purpose is to tell treasure when it's done opening 
    public Treasure treasure; 

    public void OpeningAnimationFinished()
    {
        treasure.OpeningAnimationFinished(); 
    }
}
