using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserShip : Ship
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void ShipHitEffect()
    {
        Debug.Log("User Ship Hit!");
    }
}
