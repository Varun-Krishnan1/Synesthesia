using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCube : ActionItem
{

    public override void DoAction()
    {
        GameManager.Instance.QuitGame(); 
    }
}
