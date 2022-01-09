using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class UnderwaterObject : MonoBehaviour
{
    public float duration; 
    public Vector3 force;
    public int vibration;
    public float randomness; 
    public bool addForce;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOShakePosition(duration, force, vibration, randomness).SetLoops(-1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
