using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int playerCurrentHealth = 3, playerMaxHealth = 5;
    public float invincibilityDuration = 0.5f, moveSpeedIncrease = 2f; // Duration of invincibility in seconds
    public bool shotsOnDamage, goldenMiddle;
    private bool isInvincible, goldenMiddleOn;
    PlayerMovement playerMovement;
    PlayerDeath playerDeath;
    Weapon weapon;

    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerDeath = gameObject.GetComponent<PlayerDeath>();
        weapon = GetComponent<Weapon>();
    }

    public void DealDamage(int damageDone)
    {
        if (!isInvincible)
        {
            transform.Find("Particles").Find("Damage Particles").GetComponent<ParticleSystem>().Play();
            playerCurrentHealth -= damageDone;
            GetComponent<UIController>().UpdateHearts();
            //Debug.Log("took " + damageDone + " damage");
            if (playerCurrentHealth <= 0 || playerCurrentHealth >= playerMaxHealth)
                playerDeath.Die();
            else{
            if (shotsOnDamage)
                weapon.ShotsOnDamage();
            if (goldenMiddle)
                GoldenMiddle();
            playerMovement.moveSpeed += moveSpeedIncrease;
            StartCoroutine(InvincibilityCoroutine());}
        }
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        gameObject.layer = LayerMask.NameToLayer("Invincible Player");
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        gameObject.layer = LayerMask.NameToLayer("Player");
        playerMovement.moveSpeed -= moveSpeedIncrease;
    }

    private void GoldenMiddle()
    {
        if ((Math.Floor((decimal) playerMaxHealth) == playerCurrentHealth || Math.Ceiling((decimal) playerMaxHealth) == playerCurrentHealth) && !goldenMiddleOn)
        {
            weapon.damage += 2;
            weapon.attackSpeedMult += 0.5f;
            goldenMiddle = true;
        }
        else if ((Math.Floor((decimal) playerMaxHealth) != playerCurrentHealth && Math.Ceiling((decimal) playerMaxHealth) != playerCurrentHealth) &&goldenMiddle)
        {
            weapon.damage -= 2;
            weapon.attackSpeedMult -= 0.5f;
            goldenMiddle = false;
        }
    }
}