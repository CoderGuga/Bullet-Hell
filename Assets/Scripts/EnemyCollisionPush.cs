using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollisionPush : MonoBehaviour
{
    public float pushForce = 10f;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            Rigidbody2D otherRb = collider.GetComponent<Rigidbody2D>();
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 direction = (rb.position - otherRb.position).normalized;
            rb.AddForce(pushForce*direction);
        }
    }
}
