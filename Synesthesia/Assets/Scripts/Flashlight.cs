using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject light;

    private bool lightOn; 
    
    public void ToggleLight()
    {
        if (lightOn)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }

    void TurnOff()
    {
        light.SetActive(false);
        lightOn = false; 
    }

    void TurnOn()
    {
        light.SetActive(true);
        lightOn = true;
    }
}
