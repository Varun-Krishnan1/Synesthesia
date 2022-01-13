using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public GameObject ship; // -- keep track of which ship it came from
    public float damage; 
    private float ballForce;
    public float colorCloudScale; 

    public void SetBallForce(float force)
    {
        ballForce = force; 
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CannonCollision")
        {
            // -- ensure it doesn't hit collider of same ship it came from 
            if(other.gameObject.transform.parent.gameObject != ship)
            {
                VisualManager.Instance.DrawColorSplash(transform.position, transform.rotation, new Vector3(colorCloudScale, colorCloudScale, colorCloudScale), Drum.drumTypes.Kick);
                
                if (other.gameObject.GetComponent<Shield>())
                {
                    other.gameObject.GetComponent<Shield>().HitEffect(damage);
                }
                else
                {
                    other.gameObject.transform.parent.GetComponent<Ship>().HitEffect(damage);
                }


                Destroy(this.gameObject);
            }

        }
    }
}
