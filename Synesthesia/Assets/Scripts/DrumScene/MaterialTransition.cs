using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialTransition : MonoBehaviour
{
    public Material material2;

    public float speed = 1f;
    public float delay = 5f;
    public float transitionTime = 8f;
    Renderer rend;

    private Material material1;
    private float startTime;
    private bool doneTransitioning;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        material1 = rend.material;

        startTime = Time.time;

    }

    public void SetMaterial(Material material)
    {
        material2 = material; 
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime > delay && !doneTransitioning)
        {
            if (Time.time - startTime > transitionTime)
            {
                rend.material = material2;
                doneTransitioning = true;
            }
            // -- transition materials 
            else
            {
                rend.material.Lerp(material1, material2, Time.deltaTime * speed);
            }
            //Debug.Log("started");
        }
    }
}
