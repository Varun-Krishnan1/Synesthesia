using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodInteractable : ActionItem
{
    public override void DoAction()
    {
        AudioManager.Instance.PlaySoundEffect(19, 0.5f, .17f);
    }

}
