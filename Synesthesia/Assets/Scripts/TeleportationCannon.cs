using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationCannon : MonoBehaviour
{

    public GameObject arrow;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideArrow()
    {
        arrow.SetActive(false);
    }
}