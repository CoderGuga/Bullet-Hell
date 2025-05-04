using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class BuffBubble : MonoBehaviour
{
    List<bool> buffs;
    public bool
    upDamage;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Buffs buffs = collision.GetComponent<Buffs>();
            if (upDamage)
                UpDamage(buffs);

            Destroy(gameObject);
        }
    }


    void UpDamage(Buffs buffs)
    {
        buffs.UpDamage();
    }
}
