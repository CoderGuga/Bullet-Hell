using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeeleDamage : MonoBehaviour
{
    public int damage = 1;
    public bool feed;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
            DamagePlayer(collider.GetComponent<PlayerHealth>());
    }

    void DamagePlayer(PlayerHealth playerHealth)
    {
        if (feed)
            damage *= -1;
        playerHealth.DealDamage(damage);
    }
}
