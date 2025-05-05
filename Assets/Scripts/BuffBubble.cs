using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class BuffBubble : MonoBehaviour
{
    List<bool> buffs;
    public bool
    upDamage,
    upBounces,
    upPiercing,
    upMaxHealth,
    upAttackSpeed,
    upMovementSpeed;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Buffs buffs = collision.GetComponent<Buffs>();
            collision.transform.Find("Particles").Find("Buff Particles").GetComponent<ParticleSystem>().Play();
            if (buffs != null)
            {
                if (upDamage)
                    UpDamage(buffs);
                if (upBounces)
                    UpBounces(buffs);
                if (upPiercing)
                    UpPiercing(buffs);
                if (upMaxHealth)
                    UpMaxHealth(buffs);
                if (upAttackSpeed)
                    UpAttackSpeed(buffs);
                if (upMovementSpeed)
                    UpMovementSpeed(buffs);
            }

            Destroy(gameObject);
        }
    }


    void UpDamage(Buffs buffs)
    {
        buffs.UpDamage();
    }

    void UpBounces(Buffs buffs)
    {
        buffs.UpBounces();
    }

    void UpPiercing(Buffs buffs)
    {
        buffs.UpPiercing();
    }

    void UpMaxHealth(Buffs buffs)
    {
        buffs.UpMaxHealth();
    }

    void UpAttackSpeed(Buffs buffs)
    {
        buffs.UpAttackSpeed();
    }

    void UpMovementSpeed(Buffs buffs)
    {
        buffs.UpMovementSpeed();
    }
}
