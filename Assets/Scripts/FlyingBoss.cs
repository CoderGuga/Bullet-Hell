using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBoss : MonoBehaviour
{
    public GameObject player, projectilePrefab, enemyPrefab;
    public EnemySpawner enemySpawner;
    public float 
    dashForce, 
    dashTime, 
    shootTime, 
    dashRecovery, 
    shootRecovery,
    spawnRecovery,
    shotForce = 10f,
    aliveTimer = 5,
    pushForce;

    public int 
    damage = 1, 
    piercing = 1, 
    bounces;
    
    public bool feed;


    float timer, nextAttackTime = 4f, totalSpreadAngle, inaccuracyAngle;
    int projectilesPerShot = 1;
    List<string> attacks = new List<string>(){"Dash", "ShootAttack", "SpawnEnemies"};
    List<GameObject> shotProjectiles = new List<GameObject>();
    Rigidbody2D rb; EnemyCollisionPush enemyCollisionPush;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyCollisionPush = GetComponent<EnemyCollisionPush>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextAttackTime)
            ChooseNextAttack();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Rigidbody2D>())
        {
            Rigidbody2D otherRb = collider.GetComponent<Rigidbody2D>();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 direction = (otherRb.position - rb.position).normalized;
            otherRb.AddForce(pushForce*direction * otherRb.mass);
        }
    }

    void ChooseNextAttack()
    {
        string nextAttack = attacks[Random.Range(0, attacks.Count)];
        StartCoroutine(nextAttack);
    }

    IEnumerator Dash()
    {
        timer = 0;
        nextAttackTime = dashRecovery;
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("dashing");
            //play anim
            yield return new WaitForSeconds(1f);
            Vector2 dir = (player.transform.position - transform.position).normalized;
            rb.velocity = Vector2.zero;
            pushForce *= 5;
            rb.AddForce(dir * dashForce);
            yield return new WaitForSeconds(dashTime);
            rb.velocity = Vector2.zero;
            pushForce /= 5;
        }
    }

    IEnumerator ShootAttack()
    {
        timer = 0;
        nextAttackTime = shootRecovery;
        Debug.Log("shooting");
        //play anim
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 3; i++)
        {
            projectilesPerShot = 5 - i;
            totalSpreadAngle = projectilesPerShot * 10;
            Shoot();
            yield return new WaitForSeconds(shootTime);
        }
    }

    IEnumerator SpawnEnemies()
    {
        timer = 0;
        nextAttackTime = spawnRecovery;
        GameObject enemy;
        for (int i = 0; i < 3; i++)
        {
            enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemySpawner.enemyList.Add(enemy);
            if (enemy.GetComponent<EnemyMovement>())
                enemy.GetComponent<EnemyMovement>().target = player;
            if (enemy.GetComponent<EnemyShooting>())
                enemy.GetComponent<EnemyShooting>().target = player;

            enemy.GetComponent<Rigidbody2D>().AddForce((player.transform.position - transform.position).normalized * 500);
            yield return new WaitForSeconds(0.8f);
            enemy.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void Die()
    {
        foreach(GameObject enemy in enemySpawner.enemyList)
        {
            Destroy(enemy);
        }

        foreach(GameObject projectile in shotProjectiles)
        {
            Destroy(projectile);
        }
        enemySpawner.enemyList.Clear();
        Destroy(gameObject);
        enemySpawner.EndWave();
    }

    

    void Shoot()
    {        
            float spread = totalSpreadAngle/(projectilesPerShot-1);
            float currentAngle = totalSpreadAngle/2;
            for (int i = 0; i < projectilesPerShot; i++)
            {
                float currentInaccuracy = Random.Range(-inaccuracyAngle, inaccuracyAngle);

                Quaternion projectileRotation = Quaternion.Euler(0f, 0f, currentAngle+currentInaccuracy);
                Vector2 direction = (Vector2)(player.transform.position - transform.position).normalized;
                GameObject projectile = Instantiate(projectilePrefab, transform.position, projectileRotation);
                shotProjectiles.Add(projectile);

                EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                rb.AddForce(projectileRotation * direction * shotForce); // Apply force to the bullet
                currentAngle-=spread;
                ApplyGoodies(enemyProjectile);
                //Debug.Log("CurrentAngle" + currentAngle);
            }
    }

    void ApplyGoodies(EnemyProjectile enemyProjectile)
    {
        enemyProjectile.damage = damage;
        enemyProjectile.bounces = bounces;
        enemyProjectile.piercing = piercing;
        enemyProjectile.aliveTimer = aliveTimer;
        enemyProjectile.shotForce = shotForce;
        enemyProjectile.feed = feed;
    }
}
