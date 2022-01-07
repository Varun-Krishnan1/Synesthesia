using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UserShip : Ship
{

    public Shield shield;
    public GameObject shieldIcon; 
    public GameObject[] drumIcons;

    private int shieldUsesLeft;

    void Start()
    {
        shieldUsesLeft = drumIcons.Length;
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
        // -- if shield is already up don't use one of their shield uses
        if (shieldUsesLeft > 0 && !shield.gameObject.activeSelf)
        {
            shield.gameObject.SetActive(true);
            Destroy(drumIcons[shieldUsesLeft - 1]);
            shieldUsesLeft -= 1;

            if(shieldUsesLeft == 0)
            {
                shieldIcon.GetComponent<Renderer>().material.SetColor("Base_Color", Color.black);
            }
        }

    }

    protected override void Sink()
    {
        this.transform.DOMoveY(transform.position.y - sinkDepth, sinkTime);
    }
}
