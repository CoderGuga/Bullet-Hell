using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 1f;
    public float aliveTimer = 1f, currentTime;
    public int piercing = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyComponent))
        {
            enemyComponent.TakeDamage(damage);
            piercing -= 1;
        }
        else
            Destroy(gameObject);
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= aliveTimer || piercing <= 0)
            Destroy(gameObject);
    }
}
