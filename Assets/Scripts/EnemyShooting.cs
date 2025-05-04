using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public float shotPeriod = 1f,
    shotForce = 10f,
    totalSpreadAngle = 0f,
    inaccuracyAngle = 0f;


    public int projectilesPerShot = 1;

    public GameObject projectilePrefab, target;

    //float currentTime = 0f;



    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", shotPeriod, shotPeriod);
    }

    void Shoot()
    {
        
        //if (projectilesPerShot > 1){
        
            float spread = totalSpreadAngle/(projectilesPerShot-1);
            float currentAngle = totalSpreadAngle/2;
            for (int i = 0; i < projectilesPerShot; i++)
            {
                float currentInaccuracy = Random.Range(-inaccuracyAngle, inaccuracyAngle);

                Quaternion projectileRotation = Quaternion.Euler(0f, 0f, currentAngle+currentInaccuracy);
                Vector2 direction = (Vector2)(target.transform.position - transform.position).normalized;
                GameObject projectile = Instantiate(projectilePrefab, transform.position, projectileRotation);

                EnemyProjectile enemyProjectile = projectile.GetComponent<EnemyProjectile>();
                enemyProjectile.shotForce = shotForce;
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                rb.AddForce(projectileRotation * direction * shotForce); // Apply force to the bullet
                currentAngle-=spread;
                //Debug.Log("CurrentAngle" + currentAngle);
            }
        
        /*
        }else
        {
            Vector2 direction = (Vector2)(target.transform.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, new(0,0,0,0));
            projectile.GetComponent<Rigidbody2D>().AddForce(shotForce*direction);
        }
        */
    }
}
