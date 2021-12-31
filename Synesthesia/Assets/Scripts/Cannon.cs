using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject cannonBallSet;
    public GameObject cannonBall;

    public float cannonBallForce;
    public bool fire; 

    public void Fire()
    {
        CannonBall newCannonBall = Instantiate(cannonBall, firePoint.position, firePoint.rotation).GetComponent<CannonBall>();
        newCannonBall.SetBallForce(cannonBallForce);
        newCannonBall.Fire(); 

    }

    void Update()
    {
        // -- for testing 
        if(fire)
        {
            Fire();
            fire = false; 
        }
    }
}
