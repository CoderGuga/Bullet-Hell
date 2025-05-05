using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float attackSpeed = 0.5f,
    bulletsPerShot = 3f,
    bulletForce = 20f,
    semiAutoSpeed = 0.4f,
    maxBullets = 10f,
    reloadTime = 2f,
    bulletsLeft,
    randomSpreadAngle = 20f, 
    shotgunPelletsPerShot = 5f;
    public Transform firePoint; // Position from which bullets are spawned
    public GameObject bulletPrefab; // Bullet type (from given prefab)
    public bool automatic; 
    //shotgun;
    public bool readyToShoot = true; // Flag to determine if the player is ready to shoot
    private bool reloading = false;
    [SerializeField] private bool isShooting; //to determing whether the player has given the input to shoot
    float bulletsShot = 0f; // Counter to keep track of how many bullets have been shot in the current burst
    float lastShotTime = 0f; // Track the time when the last shot was fired
    [SerializeField] bool infiniteMagazine = false;




    public int damage, piercing, bounces;
    public float aliveTimer;

    private void Start()
    {
        bulletsLeft = maxBullets;
    }

    public bool IsWeaponReadyToShoot()
    {
        return (readyToShoot && !reloading && bulletsLeft > 0) || infiniteMagazine;
    }

    private void Update()
    {

        if (automatic) isShooting = Input.GetButton("Fire1");
        else isShooting = Input.GetButtonDown("Fire1");

        if (isShooting && IsWeaponReadyToShoot())
        {
            if (Time.time - lastShotTime >= attackSpeed)
            {
                lastShotTime = Time.time;
                bulletsShot = bulletsPerShot; // Set the number of bullets to be shot in the current burst
                Shoot();
            }
        }

        if (Input.GetKeyDown("r") && !reloading && bulletsLeft != maxBullets && readyToShoot)
        {
            reloading = true;
            Invoke("Reload", reloadTime);
        }
    }

    private void Shoot()
    {
        readyToShoot = false; // Prevent further shooting until cooldown
        //if (shotgun){
        
            for (int i = 0; i < shotgunPelletsPerShot; i++)
            {
                Vector2 dir = (firePoint.position - transform.position).normalized;
                // Create a bullet with a slight variation in rotation
                float spread = Random.Range(-randomSpreadAngle, randomSpreadAngle);
                Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0f, 0f, spread);

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(bulletRotation * dir * bulletForce); // Apply force to the bullet
                if(bullet.TryGetComponent<Bullet>(out Bullet bulletSc))
                    ApplyGoodies(bulletSc);
            }
        /*
        }
        else
        {

            // Instantiate a bullet prefab at the firePoint with the desired rotation
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, new (0,0,0,0));
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Vector2 dir = (firePoint.position - transform.position).normalized;
            rb.AddForce(dir * bulletForce); // Apply force to the bullet
            Bullet bulletSc = bullet.GetComponent<Bullet>();
            ApplyGoodies(bulletSc);
        }
        */
        
        bulletsLeft--;
        bulletsShot--;

        // If there are more bullets to be shot in the current burst, schedule the Shoot() method to be called again after a delay
        if (bulletsShot > 0 && bulletsLeft != 0f)
            Invoke("Shoot", semiAutoSpeed);

        // Schedule the ResetShot() method to reset the ability to shoot after the specified cooldown
        StartCoroutine(ResetShot());
    }

    public IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(attackSpeed);
        readyToShoot = true; // Allow the player to shoot again
    }

    private void Reload()
    {
        bulletsLeft = maxBullets;
        reloading = false;
    }





    void ApplyGoodies(Bullet bullet)
    {
        bullet.damage = damage;
        bullet.bounces = bounces;
        bullet.piercing = piercing;
        bullet.aliveTimer = aliveTimer;
        bullet.shotForce = bulletForce;
    }
}
