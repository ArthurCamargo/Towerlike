using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShootTower : Tower
{
    protected override void Start() {
        base.Start();
    }

    public override void Attack() {
        Shoot(target);
    }

    public void Shoot(Transform target) {
        
        Projectile newProjectile = Instantiate(projectile, attackPlaceHolder.position, attackPlaceHolder.rotation) as Projectile;
        newProjectile.SetSpeed(attackSpeed);
        newProjectile.SetTarget(target);
        newProjectile.SetDamage(damage);
    }
}