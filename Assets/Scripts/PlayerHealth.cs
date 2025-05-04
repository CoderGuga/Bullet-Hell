using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int playerCurrentHealth = 3, playerMaxHealth = 5;
    public float invincibilityDuration = 0.5f, moveSpeedIncrease = 2f; // Duration of invincibility in seconds
    private bool isInvincible = false;
    PlayerMovement playerMovement;
    PlayerDeath playerDeath;

    void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerDeath = gameObject.GetComponent<PlayerDeath>();
    }

    public void DealDamage(int damageDone)
    {
        if (!isInvincible)
        {
            playerCurrentHealth -= damageDone;
            if (playerCurrentHealth <= 0 || playerCurrentHealth >= playerMaxHealth)
                playerDeath.Die();
            playerMovement.moveSpeed += moveSpeedIncrease;
            StartCoroutine(InvincibilityCoroutine());
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
}