using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float timeBetweenShooting = 0.5f; // Time delay between shooting consecutive sets of bullets
    [SerializeField] float bulletsPerShot = 3f; // Number of bullets fired simultaneously in a single shot
    [SerializeField] Transform firePoint; // Position from which bullets are spawned
    [SerializeField] GameObject bulletPrefab; // Bullet type (from given prefab)
    [SerializeField] float bulletForce = 20f; // Initial bullet speed
    [SerializeField] float timeBetweenSameShootingShots = 0.4f; // Time delay between individual bullets within the same shot
    [SerializeField] float maxBullets = 10f;
    [SerializeField] float reloadTime = 2f;
    [SerializeField] float bulletsLeft;
    [SerializeField] float shotgunSpreadAngle = 20f; // The angle of the spread for shotgun pellets
    [SerializeField] float shotgunPelletsPerShot = 5f;
    [SerializeField] bool automatic, shotgun; // Weapon type
    public bool readyToShoot = true; // Flag to determine if the player is ready to shoot
    private bool reloading = false;
    [SerializeField] private bool isShooting; //to determing whether the player has given the input to shoot
    float bulletsShot = 0f; // Counter to keep track of how many bullets have been shot in the current burst
    float lastShotTime = 0f; // Track the time when the last shot was fired
    [SerializeField] bool infiniteMagazine = false;

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
            if (Time.time - lastShotTime >= timeBetweenShooting)
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
        if (shotgun)
        {
            for (int i = 0; i < shotgunPelletsPerShot; i++)
            {
                Vector2 dir = (firePoint.position - transform.position).normalized;
                // Create a bullet with a slight variation in rotation
                float spread = Random.Range(-shotgunSpreadAngle, shotgunSpreadAngle);
                Quaternion bulletRotation = firePoint.rotation * Quaternion.Euler(0f, 0f, spread);

                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, bulletRotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.AddForce(bulletRotation * dir * bulletForce); // Apply force to the bullet
            }
        }
        else
        {

            // Instantiate a bullet prefab at the firePoint with the desired rotation
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, new (0,0,0,0));
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Vector2 dir = (firePoint.position - transform.position).normalized;
            rb.AddForce(dir * bulletForce); // Apply force to the bullet
        }
        
        bulletsLeft--;
        bulletsShot--;

        // If there are more bullets to be shot in the current burst, schedule the Shoot() method to be called again after a delay
        if (bulletsShot > 0 && bulletsLeft != 0f)
            Invoke("Shoot", timeBetweenSameShootingShots);

        // Schedule the ResetShot() method to reset the ability to shoot after the specified cooldown
        StartCoroutine(ResetShot());
    }

    public IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(timeBetweenShooting);
        readyToShoot = true; // Allow the player to shoot again
    }

    private void Reload()
    {
        bulletsLeft = maxBullets;
        reloading = false;
    }
}
