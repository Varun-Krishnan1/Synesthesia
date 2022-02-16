using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UserShip : Ship
{

    public Shield shield;
    public GameObject shieldIcon; 
    public GameObject[] drumIcons;

    public float comboCannonDamage;

    private int shieldUsesLeft;

    void Start()
    {
        shieldUsesLeft = drumIcons.Length;
    }

    protected override void ShipHitEffect()
    {
        Debug.Log("User Ship Hit!");
    }

    public void Shoot()
    {
        int randNum = Random.Range(0, cannons.Length);
        foreach (Transform child in cannons[randNum].transform)
        {
            child.GetComponent<Cannon>().damage = cannonDamage;
            child.GetComponent<Cannon>().Fire(cannonSounds[Random.Range(0, cannonSounds.Length - 1)], impactSounds[Random.Range(0, impactSounds.Length - 1)]);
        }
    }

    public void ComboShoot()
    {
        int randNum = Random.Range(0, cannons.Length);
        foreach (Transform child in cannons[randNum].transform)
        {
            child.GetComponent<Cannon>().damage = comboCannonDamage;
            child.GetComponent<Cannon>().Fire(cannonSounds[Random.Range(0, cannonSounds.Length - 1)], impactSounds[Random.Range(0, impactSounds.Length - 1)]);
        }
    }

    public void SetShield()
    {
        // -- if shield is already up don't use one of their shield uses
        if (shieldUsesLeft > 0 && !shield.gameObject.activeSelf)
        {
            shield.gameObject.SetActive(true);
            Destroy(drumIcons[shieldUsesLeft - 1]);
            shieldUsesLeft -= 1;

            if(shieldUsesLeft == 0)
            {
                shieldIcon.GetComponent<Renderer>().material.SetColor("Base_Color", Color.black);
            }
        }

    }

    void Update()
    {

    }
    protected override void Sink()
    {
        AudioManager.Instance.PlaySoundEffect(8, .5f, .2f);
        StartCoroutine(SinkCoroutine());
    }

    IEnumerator SinkCoroutine()
    {
        // -- give some time after user health goes down to
        yield return new WaitForSeconds(5f);
        StageTwo.Instance.StageOver(win: false);
    }
}
