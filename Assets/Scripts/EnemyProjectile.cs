using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 1;
    public float aliveTimer = 1f, currentTime, shotForce;
    public int piercing = 1, bounces = 0;
    public bool feed;
    public Vector2 velocity;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Wall")
        {
            if (bounces > 0){
                Bounce(collision);
            }
            else
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Wall")
        {
            if (collision.gameObject.TryGetComponent(out PlayerHealth playerHealth))
            {
                if (feed)
                    damage *= -1;
                Debug.Log("dealt " + damage + " damage");
                playerHealth.DealDamage(damage);
                piercing --;
            }
            else if (collision.gameObject.TryGetComponent(out Bullet bullet))
            {
                piercing --;
            }
            else
                Destroy(gameObject);
        }

        //Debug.Log("projectile collided");
    }

    Vector2 RotateVector(Vector2 vector, float angle)
    {
        float radian = angle * Mathf.Deg2Rad; // Convert angle to radians
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);

        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }

    public void UpdateVelocity()
    {
        velocity = rb.velocity;
    }

    public float SignedAngle(Vector2 from, Vector2 to)
    {
        float cross = from.x * to.y - from.y * to.x; // 2D cross product
        float dot = Vector2.Dot(from, to);          // Dot product
        return Mathf.Atan2(cross, dot) * Mathf.Rad2Deg; // atan2 gives the signed angle
    }

    void Bounce(Collision2D collision)
    {
        //Debug.Log("Boing!");
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 normal = contact.normal;
            //float angle = 180 - Vector2.Angle(normal, velocity);
            float angle = 180 - SignedAngle(normal, velocity);
            //Debug.Log("velocity: " + rb.velocity);
            rb.velocity = Vector2.zero; 
            Vector2 dir = RotateVector(normal, angle);
            rb.AddForce(dir * shotForce);
            //Debug.Log($"Normal: {normal} Angle: {angle} dir: {dir}");
            
            bounces --;
    }

    void Update()
    {
        UpdateVelocity();
        currentTime += Time.deltaTime;
        if (currentTime >= aliveTimer || piercing <= 0)
            Destroy(gameObject);
    }
}
