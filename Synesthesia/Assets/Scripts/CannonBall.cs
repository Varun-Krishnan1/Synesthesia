using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
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
            VisualManager.Instance.DrawColorSplash(transform.position, transform.rotation, Drum.drumTypes.Snare);
            other.gameObject.transform.parent.GetComponent<Ship>().HitEffect(damage);

            Destroy(this.gameObject); 
        }
    }
}
