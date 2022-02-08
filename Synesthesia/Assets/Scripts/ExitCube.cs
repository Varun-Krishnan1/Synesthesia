using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCube : ActionItem
{

    public override void DoAction()
    {
        Debug.Log("User has quit!");
        Application.Quit();
    }
}
