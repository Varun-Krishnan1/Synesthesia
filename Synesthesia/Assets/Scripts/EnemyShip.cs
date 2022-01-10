using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyShip : Ship
{
    public float speedFactor; 
    private float nextShootTime;

    public bool activated; 
    // Update is called once per frame
    void Update()
    {
        if(activated)
        {
            // -- initialize next shoot time to correspond to beat manager 
            if (nextShootTime == 0f)
            {
                nextShootTime = BeatManager.Instance.nextPlayTime;
            }

            if (Time.time > nextShootTime)
            {
                Shoot();
                nextShootTime = Time.time + (AudioManager.Instance.secPerBeat * speedFactor);
            }
        }

    }

    protected override void ShipHitEffect()
    {
        Debug.Log("Enemy Ship Hit!");

    }

    void Shoot()
    {
        int randNum = Random.Range(0, cannons.Length);
        foreach (Transform child in cannons[randNum].transform)
        {
            child.GetComponent<Cannon>().Fire(); 
        }
    }

    protected override void Sink()
    {
        this.transform.DOMoveY(transform.position.y-sinkDepth, sinkTime);

    }
}
