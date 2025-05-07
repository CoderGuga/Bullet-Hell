using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class BuffBubble : MonoBehaviour
{
    public BuffSpawner buffSpawner;
    Buffs buffs;
    public bool
    upDamage,
    upBounces,
    upPiercing,
    upMaxHealth,
    upAttackSpeed,
    upMovementSpeed,
    shootingPet,
    dashingPet,
    invincibilityDurationUp,
    shotsPerAttackUp,
    damagePerHp,
    speedPerHp,
    upAccuracy,
    bulletsOnDamageTake,
    destroyableEnemyBullets,
    delayedDamage,
    goldenMiddle,
    bulletsSplit,
    infuseBullets,
    explodingEnemies,
    eatCorpses,
    bulletSpeedUp;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            buffs = collision.GetComponent<Buffs>();
            collision.transform.Find("Particles").Find("Buff Particles").GetComponent<ParticleSystem>().Play();


            if (upDamage) UpDamage();
            if (upBounces) UpBounces();
            if (upPiercing) UpPiercing();
            if (upMaxHealth) UpMaxHealth();
            if (upAttackSpeed) UpAttackSpeed();
            if (upMovementSpeed) UpMovementSpeed();
            if (shootingPet) ShootingPet();
            if (bulletsOnDamageTake) ShotsOnDamage();
            if (invincibilityDurationUp) UpAfterHit();
            if (bulletSpeedUp) BulletSpeedUp();
            if (shotsPerAttackUp) ShotsPerAttackUp();
            if (damagePerHp) DamagePerHp();
            if (speedPerHp) SpeedPerHp();

                buffSpawner.DestoyBuffBubbles();
                Debug.Log("Buff Taken");
            Destroy(gameObject);
        }
    }


    void UpDamage()
    {
        buffs.UpDamage();
    }

    void UpBounces()
    {
        buffs.UpBounces();
    }

    void UpPiercing()
    {
        buffs.UpPiercing();
    }

    void UpMaxHealth()
    {
        buffs.UpMaxHealth();
    }

    void UpAttackSpeed()
    {
        buffs.UpAttackSpeed();
    }

    void UpMovementSpeed()
    {
        buffs.UpMovementSpeed();
    }

    void ShootingPet()
    {
        buffs.ShootingPet();
    }

    void ShotsOnDamage()
    {
        buffs.ShotsOnDamage();
        RemoveFromList();
    }

    void UpAfterHit()
    {
        buffs.UpAfterHit();
    }

    void BulletSpeedUp()
    {
        buffs.BulletSpeedUp();
    }

    void ShotsPerAttackUp()
    {
        buffs.ShotsPerAttackUp();
    }

    void DamagePerHp()
    {
        buffs.DamagePerHp();
        RemoveFromList();
    }

    void SpeedPerHp()
    {
        buffs.SpeedPerHp();
        RemoveFromList();
    }



    void RemoveFromList()
    {
        buffSpawner.buffBubbles.Remove(gameObject);
    }
}
