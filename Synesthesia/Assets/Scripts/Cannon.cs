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

    private AudioSource source; 
    void Start()
    {
        if(sideCannon)
        {
            firePoint = this.gameObject.transform; 
        }

        source = GetComponent<AudioSource>(); 
    }
    public void Fire(AudioClip cannonSound, AudioClip impactSound)
    {
        CannonBall newCannonBall = Instantiate(cannonBall, firePoint.position, firePoint.rotation).GetComponent<CannonBall>();
        newCannonBall.SetBallForce(cannonBallForce);
        newCannonBall.damage = damage;
        newCannonBall.ship = ship;
        newCannonBall.colorCloudScale = colorCloudScale;
        newCannonBall.source.clip = impactSound;
        newCannonBall.gameObject.GetComponent<Rigidbody>().AddForce(firePoint.right * cannonBallForce, ForceMode.Impulse);

        source.clip = cannonSound; 
        source.Play(); 
    }

    void Update()
    {
    }
}
