using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserShip : Ship
{

    public Shield shield; 
    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void ShipHitEffect()
    {
        Debug.Log("User Ship Hit!");
    }

    public void Shoot()
    {
        int randNum = Random.Range(0, cannons.Length);
        foreach (Transform child in cannons[randNum].transform)
        {
            child.GetComponent<Cannon>().Fire();
        }
    }

    public void SetShield()
    {
        shield.gameObject.SetActive(true); 
    }
    protected override void Sink()
    {

    }
}
