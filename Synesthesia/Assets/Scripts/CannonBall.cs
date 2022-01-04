using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public GameObject ship; // -- keep track of which ship it came from
    public int damage; 
    private float ballForce; 

    public void SetBallForce(float force)
    {
        ballForce = force; 
    }

    public void Fire()
    {
        this.GetComponent<Rigidbody>().AddForce(Vector3.right * ballForce, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CannonCollision")
        {
            // -- ensure it doesn't hit collider of same ship it came from 
            if(other.gameObject.transform.parent.gameObject != ship)
            {
                VisualManager.Instance.DrawColorSplash(transform.position, transform.rotation, Drum.drumTypes.Snare);
                other.gameObject.transform.parent.GetComponent<Ship>().HitEffect(damage);

                Destroy(this.gameObject);
            }

        }
    }
}
