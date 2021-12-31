using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ship : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
}
