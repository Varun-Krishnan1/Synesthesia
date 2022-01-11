using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject ship; 
    public Transform firePoint;
    public GameObject cannonBall;

    public float cannonBallForce;
    public float damage;
    public float colorCloudScale;
    public bool sideCannon; 
    public bool fire; 

    void Start()
    {
        if(sideCannon)
        {
            firePoint = this.gameObject.transform; 
        }
    }
    public void Fire()
    {
        CannonBall newCannonBall = Instantiate(cannonBall, firePoint.position, firePoint.rotation).GetComponent<CannonBall>();
        newCannonBall.SetBallForce(cannonBallForce);
        newCannonBall.damage = damage;
        newCannonBall.ship = ship;
        newCannonBall.colorCloudScale = colorCloudScale;
        newCannonBall.gameObject.GetComponent<Rigidbody>().AddForce(firePoint.right * cannonBallForce, ForceMode.Impulse);
      
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
