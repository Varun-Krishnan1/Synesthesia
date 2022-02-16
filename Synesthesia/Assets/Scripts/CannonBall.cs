using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public GameObject ship; // -- keep track of which ship it came from
    public float damage; 
    private float ballForce;
    public float colorCloudScale;

    public Color UserShipCannonColor;
    public Color EnemyShipCannonColor;

    private bool activated;
    public AudioSource source;

    public void SetBallForce(float force)
    {
        ballForce = force; 
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "CannonCollision")
        {
            // -- ensure it doesn't hit collider of same ship it came from 
            if(!activated && other.gameObject.transform.parent.gameObject != ship)
            {
                Color colorCloudColor; 
                // -- if enemy ball hitting user ship 
                if(other.gameObject.transform.parent.GetComponent<Ship>().isUserShip)
                {
                    colorCloudColor = EnemyShipCannonColor;
                }
                // -- else user ship hitting enemy ship 
                else
                {
                    colorCloudColor = UserShipCannonColor;
                }

                VisualManager.Instance.DrawColorSplash(transform.position, transform.rotation, new Vector3(colorCloudScale, colorCloudScale, colorCloudScale), colorCloudColor);
                
                if (other.gameObject.GetComponent<Shield>())
                {
                    other.gameObject.GetComponent<Shield>().HitEffect(damage);
                }
                else
                {
                    other.gameObject.transform.parent.GetComponent<Ship>().HitEffect(damage);
                }

                source.time = .3f; 
                source.Play();

                // -- destroy object after 5 seconds so impact sound can still play 
                this.GetComponent<MeshRenderer>().enabled = false; 
                Destroy(this.gameObject, 5f);

                activated = true; 
            }

        }
    }
}
