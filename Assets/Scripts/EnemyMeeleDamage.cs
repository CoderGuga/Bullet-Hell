using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeeleDamage : MonoBehaviour
{
    public int damage;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
            DamagePlayer(collider.GetComponent<PlayerHealth>());
    }

    void DamagePlayer(PlayerHealth playerHealth)
    {
        playerHealth.DealDamage(damage);
    }
}
