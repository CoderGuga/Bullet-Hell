using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float attackSpeed = 0.5f,
    bulletForce = 20f,
    semiAutoSpeed = 0.4f,
    randomSpreadAngle = 5f,
    totalSpreadAngle = 0f,
    movementSpreadPenaltyMult = 2,
    stabilizationTime = 2,
    shotgunPelletsPerShot = 5f,
    attackSpeedMult = 1;
    public Transform firePoint; // Position from which bullets are spawned
    public GameObject bulletPrefab; // Bullet type (from given prefab)
    public bool damagePerHp;
    float lastShotTime = 0f;
    PlayerHealth playerHealth;




    public int damage, piercing, bounces;
    public float aliveTimer, currentAttackSpeed;

    float currentMovementSpreadPenalty = 1;


    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (currentMovementSpreadPenalty > 1)
        {
            currentMovementSpreadPenalty -= (movementSpreadPenaltyMult - 1) / stabilizationTime * Time.deltaTime;
        }

        currentAttackSpeed = attackSpeed / attackSpeedMult;
        if (Input.GetButton("Fire1"))
        {
            if (Time.time - lastShotTime >= currentAttackSpeed)
            {
                lastShotTime = Time.time;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        float spread = totalSpreadAngle / (shotgunPelletsPerShot - 1) * currentMovementSpreadPenalty;
        float currentAngle = totalSpreadAngle / 2;

        for (int i = 0; i < shotgunPelletsPerShot; i++)
        {
            Vector2 dir = (firePoint.position - transform.position).normalized;
            float randomSpread;
            if (randomSpreadAngle > 0)
                randomSpread = UnityEngine.Random.Range(-randomSpreadAngle, randomSpreadAngle) * currentMovementSpreadPenalty;
            else
                randomSpread = 0;
            Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0f, 0f, currentAngle + randomSpread);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(bulletRotation * dir * bulletForce); // Apply force to the bullet
            if (bullet.TryGetComponent<Bullet>(out Bullet bulletSc))
                ApplyGoodies(bulletSc);
            currentAngle -= spread;
        }

        GetComponent<PlayerMovement>().Knockback();

    }


    public void ShotsOnDamage()
    {
        float spread = 90;
        float currentAngle = 135;
        
            for (int i = 0; i < 4; i++)
            {
                Vector2 dir = Vector2.up;
                Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0f, 0f, currentAngle);

                GameObject bullet = Instantiate(bulletPrefab, transform.position, bulletRotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(bulletRotation * dir * bulletForce); // Apply force to the bullet
                if(bullet.TryGetComponent<Bullet>(out Bullet bulletSc))
                    ApplyGoodies(bulletSc);
                currentAngle-=spread;
            }
    }


    public void Destabilize()
    {
        currentMovementSpreadPenalty = movementSpreadPenaltyMult;
    }

    




    public void ApplyGoodies(Bullet bullet)
    {
        bullet.damage = damage;
        bullet.bounces = bounces;
        bullet.piercing = piercing;
        bullet.aliveTimer = aliveTimer;
        bullet.shotForce = bulletForce;
        bullet.GetComponent<ParticleSystem>().Play();

        if (damagePerHp)
            bullet.damage += playerHealth.playerCurrentHealth - 1;
    }



}
