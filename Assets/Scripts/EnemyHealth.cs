using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;
    public int spawnWeight = 1;
    public EnemySpawner enemySpawner;
    // Start is called before the first frame update
    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if(health <= 0)
        {
            if (enemySpawner != null && enemySpawner.enemyList.Contains(gameObject))
            {
                enemySpawner.enemyList.Remove(gameObject);
                enemySpawner.currentSpawnWeight -= spawnWeight;
                enemySpawner.CheckWave();
            }
            
            if (GetComponent<EnemyOnDeath>())
                {
                    EnemyOnDeath enemyOnDeath = GetComponent<EnemyOnDeath>();
                    enemyOnDeath.target = GetComponent<EnemyMovement>().target;
                    enemyOnDeath.EnemyDie();
                }
            Destroy(gameObject);
        }
    }


}
