using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : LivingEntity
{
    public override void TakeAttack(Attack attack)
    {
        health -= attack.damage;

        if (health <= 0 && !dead)
        {
            Die();
        }
    }
}
