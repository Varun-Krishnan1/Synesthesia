using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LowPolyUnderwaterPack;

public class FallenObject : MonoBehaviour
{
    public float delay; 
    public Vector3 force; 
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Buoyancy>())
        {
            GetComponent<Buoyancy>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(StageThree.Instance.isUnderwater)
        {
            StartCoroutine(Float());
        }
    }

    IEnumerator Float()
    {
        yield return new WaitForSeconds(delay); 

        if(GetComponent<Buoyancy>())
        {
            GetComponent<Buoyancy>().enabled = true; 
        }
        transform.parent = null;
        GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        Destroy(this);
    }
}
