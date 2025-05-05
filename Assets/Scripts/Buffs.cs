using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffs : MonoBehaviour
{
    public void UpDamage()
    {
        if (TryGetComponent<Weapon>(out Weapon weapon))
        {
            weapon.damage ++;
        }
    }

    public void UpBounces()
    {
        if (TryGetComponent<Weapon>(out Weapon weapon))
        {
            weapon.bounces ++;
        }
    }

    public void UpPiercing()
    {
        if (TryGetComponent<Weapon>(out Weapon weapon))
        {
            weapon.piercing ++;
        }
    }

    public void UpMaxHealth()
    {
        if (TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.playerMaxHealth ++;
        }
    }

    public void UpAttackSpeed()
    {
        if (TryGetComponent<Weapon>(out Weapon weapon))
        {
            weapon.attackSpeed /= 1.2f;
        }
    }

    public void UpMovementSpeed()
    {
        if (TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            playerMovement.moveSpeed *= 1.2f;
        }
    }

    public void UpAfterHit()
    {
        if (TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.invincibilityDuration *= 1.5f;
            playerHealth.moveSpeedIncrease *= 1.5f;
        }
    }
}
