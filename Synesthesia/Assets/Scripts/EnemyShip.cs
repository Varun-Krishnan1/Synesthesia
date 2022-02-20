using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyShip : Ship
{
    public float numBeatTillShoot;
    public float beatsTillFirstTransition;  
    public float beatsTillSecondTransition; 
    public float beatsTillThirdTransition; 
    public float beatsTillFourthTransition; 

    public bool activated;


    private float startDelayInBeats;
    void Start()
    {
        startDelayInBeats = StageTwo.Instance.startDelayInBeats;
    }
    // Update is called once per frame
    public void BeatChecker(float numBeats)
    {
        if(activated)
        {
            if((numBeats - startDelayInBeats) % numBeatTillShoot == 0)
            {
                Shoot(); 
            }
        }

        if (numBeats == beatsTillFirstTransition)
        {
            Shoot();
            numBeatTillShoot = 8;
        }
        else if (numBeats == beatsTillSecondTransition)
        {
            Shoot(); 
            activated = false;
        }        
        else if (numBeats == beatsTillThirdTransition)
        {
            Shoot();
            numBeatTillShoot = 12;
            activated = true; 
        }
        else if (numBeats == beatsTillFourthTransition)
        {
            Shoot();
            numBeatTillShoot = 4; 
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
            child.GetComponent<Cannon>().damage = cannonDamage; 
            child.GetComponent<Cannon>().Fire(cannonSounds[Random.Range(0, cannonSounds.Length - 1)], impactSounds[Random.Range(0, impactSounds.Length - 1)]); 
        }
    }

    protected override void Sink()
    {
        AudioManager.Instance.PlaySoundEffect(8, .5f, .2f);
        StartCoroutine(SinkCoroutine()); 
    }

    IEnumerator SinkCoroutine()
    {
        BeatManager.Instance.activated = false;

        this.transform.DOMoveY(transform.position.y - sinkDepth, sinkTime);

        yield return new WaitForSeconds(sinkTime);

        StageTwo.Instance.StageOver(win: true); 
    }
}
