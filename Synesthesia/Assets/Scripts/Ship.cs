using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

abstract public class Ship : MonoBehaviour
{
    public bool isUserShip;
    public Slider healthBar;
    public float health;
    public GameObject[] cannons;

    public float cannonDamage;
    public float sinkDepth;
    public float sinkTime;
    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = health;
        healthBar.value = healthBar.maxValue; 
    }

    public void Shake()
    {
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, 1.5f), .125f));
        mySequence.Append(transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, -1.5f), .125f));
        mySequence.Append(transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, .75f), .125f));
        mySequence.Append(transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, -.75f), .125f));
        mySequence.Append(transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, 0f), .125f));

    }

    public void CannonShake()
    {
        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, 3f), .25f));
        mySequence.Append(transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, -1.5f), .25f));   
        mySequence.Append(transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, 1.5f), .25f));
        mySequence.Append(transform.DORotate(new Vector3(0, transform.rotation.eulerAngles.y, .5f), .25f));
    }

    public void HitEffect(float damage)
    {
        health -= damage;
        healthBar.value = health;

        StageTwo.Instance.IncreaseStageIntensity(); 
        if(health <= 0)
        {
            Sink();
        }
        CannonShake();
        ShipHitEffect(); 
    }


    // -- implemented specifically for each ship 
    protected abstract void ShipHitEffect();

    protected abstract void Sink(); 
}
