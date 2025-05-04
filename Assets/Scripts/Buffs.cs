using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffs : MonoBehaviour
{
    public void UpDamage()
    {
        if (TryGetComponent<Weapon>(out Weapon weapon))
        {
            weapon.damage ++;
        }
    }
}
