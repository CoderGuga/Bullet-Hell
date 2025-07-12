using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 1f, shotForce,
    bulletSlowDownRate = 2; //how much percent of velocity it loses in a second (1 = 100%)
    public float aliveTimer = 1f, currentTime;
    public int piercing = 1, bounces;
    public bool slowDown;
    Rigidbody2D rb;
    Vector2 velocity;

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
        if (collision.gameObject.tag != "Wall"){
        if (collision.gameObject.TryGetComponent<EnemyHealth>(out EnemyHealth enemyComponent))
        {
            enemyComponent.TakeDamage(damage);
            piercing -= 1;
        }
        else
            Destroy(gameObject);
        }
    }


    void Update()
    {
        if (slowDown)
            rb.velocity -= rb.velocity * Time.deltaTime * bulletSlowDownRate;
        velocity = rb.velocity;
        currentTime += Time.deltaTime;
        if (currentTime >= aliveTimer || piercing <= 0)
            Destroy(gameObject);
    }





    void Bounce(Collision2D collision)
    {
        //Debug.Log("Boing!");
            ContactPoint2D contact = collision.GetContact(0);
            Vector2 normal = contact.normal;
            float angle = 180 - SignedAngle(normal, velocity);
            //Debug.Log("velocity: " + rb.velocity);
            rb.velocity = Vector2.zero; 
            Vector2 dir = RotateVector(normal, angle);
            rb.AddForce(dir * shotForce);
            //Debug.Log($"Normal: {normal} Angle: {angle} dir: {dir}");
            
            bounces --;
    }


    float SignedAngle(Vector2 from, Vector2 to)
    {
        float cross = from.x * to.y - from.y * to.x; // 2D cross product
        float dot = Vector2.Dot(from, to);          // Dot product
        return Mathf.Atan2(cross, dot) * Mathf.Rad2Deg; // atan2 gives the signed angle
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
}
