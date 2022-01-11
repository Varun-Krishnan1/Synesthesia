using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float shieldHealth;

    private float maxShieldHealth; 
    // Start is called before the first frame update
    void Start()
    {
        maxShieldHealth = shieldHealth; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HitEffect(float damage)
    {
        shieldHealth -= damage;
        if (shieldHealth <= 0)
        {
            // -- reset shield 
            shieldHealth = maxShieldHealth; 
            this.gameObject.SetActive(false); 
        }
    }
}
