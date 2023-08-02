using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    // dmgAmount = damage amount
    // srcObject = source of damage
    void TakeDamage(int dmgAmount, GameObject srcObject);
}
