using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

abstract public class Ship : MonoBehaviour
{
    public Slider healthBar;
    public float health; 
    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = health; 
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void HitEffect(int damage)
    {
        health -= damage;
        healthBar.value = health; 
        if(health <= 0)
        {
            Debug.Log("Ship Destroyed");
        }
        Shake();
        ShipHitEffect(); 
    }

    // -- implemented specifically for each ship 
    protected abstract void ShipHitEffect(); 
}
