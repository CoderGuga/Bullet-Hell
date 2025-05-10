using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Buffs : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public GameObject shootingPetPrefab;
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
            GetComponent<UIController>().AddHearts();
        }
    }

    public void UpAttackSpeed()
    {
        if (TryGetComponent<Weapon>(out Weapon weapon))
        {
            weapon.attackSpeedMult += 0.2f;
        }
    }

    public void UpMovementSpeed()
    {
        if (TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            playerMovement.moveSpeed *= 1.3f;
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

    public void ShootingPet()
    {
        Vector2 spawnPosition = new Vector2(transform.position.x, transform.position.y - 1);
        GameObject pet = Instantiate(shootingPetPrefab, spawnPosition, Quaternion.identity);
        pet.GetComponent<Pets>().enemySpawner = enemySpawner;
        pet.GetComponent<EnemyMovement>().target = gameObject;
    }

    public void BulletSpeedUp()
    {
        if (TryGetComponent<Weapon>(out Weapon weapon))
        {
            weapon.bulletForce *= 1.5f;
        }
    }

    public void ShotsPerAttackUp()
    {
        if (TryGetComponent<Weapon>(out Weapon weapon))
        {
            weapon.shotgunPelletsPerShot ++;
            weapon.randomSpreadAngle +=5;
        }
    }

    public void UpAccuracy()
    {
        if (TryGetComponent<Weapon>(out Weapon weapon))
        {
            weapon.randomSpreadAngle -=10;
        }
    }

    public void ShotsOnDamage()
    {
        if (TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.shotsOnDamage = true;
        }
    }

    public void DamagePerHp()
    {
        if (TryGetComponent<Weapon>(out Weapon weapon))
        {
            weapon.damagePerHp = true;
        }
    }

    public void SpeedPerHp()
    {
        if (TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
        {
            playerMovement.speedPerHp = true;
        }
    }

    public void GoldenMiddle()
    {
        if (TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.goldenMiddle = true;
        }
    }
}
