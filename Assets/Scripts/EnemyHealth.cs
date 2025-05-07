using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;
    public EnemySpawner enemySpawner;
    // Start is called before the first frame update
    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if(health <= 0 && enemySpawner.enemyList.Contains(gameObject))
        {
            enemySpawner.enemyList.Remove(gameObject);
            if (enemySpawner.enemyList.Count == 0)
            {
                Debug.Log("last enemy killed");
                enemySpawner.EndWave();
            }
            Destroy(gameObject);
        }
    }


}
