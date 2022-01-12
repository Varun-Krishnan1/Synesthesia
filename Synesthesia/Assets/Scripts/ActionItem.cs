using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoAction()
    {
        Debug.Log("Picked up!");
        TextManager.Instance.WriteText("Picked up!");
    }
}
