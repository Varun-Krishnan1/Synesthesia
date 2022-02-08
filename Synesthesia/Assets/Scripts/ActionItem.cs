using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class ActionItem : MonoBehaviour
{
    public bool doAction; // TEMPORARY 

    public abstract void DoAction();


    void Update()
    {
        if (doAction)
        {
            DoAction();
            doAction = false;
        }
    }

}
