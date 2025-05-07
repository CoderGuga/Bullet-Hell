using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pets : MonoBehaviour
{
    public bool shooting, dashing;

    public float shotPeriod = 1f,
    shotForce = 10f,
    totalSpreadAngle = 0f,
    inaccuracyAngle = 0f,
    aliveTimer = 5;


    public int projectilesPerShot = 1;

    public GameObject projectilePrefab;

    public int damage = 1, piercing = 1, bounces;

    public EnemySpawner enemySpawner;

    void Start()
    {
        if (shooting)
            InvokeRepeating("Shoot", shotPeriod, shotPeriod);
    }

    void Shoot()
    {
        GameObject target = enemySpawner.GetClosestEnemy(transform);
        if (target.name != "Player")
        {
            float spread = totalSpreadAngle/(projectilesPerShot-1);
            float currentAngle = totalSpreadAngle/2;
            for (int i = 0; i < projectilesPerShot; i++)
            {
                float currentInaccuracy = Random.Range(-inaccuracyAngle, inaccuracyAngle);

                Quaternion projectileRotation = Quaternion.Euler(0f, 0f, currentAngle+currentInaccuracy);
                Vector2 direction = (Vector2)(target.transform.position - transform.position).normalized;
                GameObject projectile = Instantiate(projectilePrefab, transform.position, projectileRotation);

                Bullet bullet = projectile.GetComponent<Bullet>();
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                rb.AddForce(projectileRotation * direction * shotForce); // Apply force to the bullet
                currentAngle-=spread;
                ApplyGoodies(bullet);
                //Debug.Log("CurrentAngle" + currentAngle);
            }
        }
        
    }

    void ApplyGoodies(Bullet bullet)
    {
        bullet.damage = damage;
        bullet.bounces = bounces;
        bullet.piercing = piercing;
        bullet.aliveTimer = aliveTimer;
        bullet.shotForce = shotForce;
    }




    
}
