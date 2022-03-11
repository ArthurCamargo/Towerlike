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
        
        Projectile newProjectile = Instantiate(projectile, placeToShoot.position, placeToShoot.rotation) as Projectile;
        newProjectile.SetSpeed(shootSpeed);
        newProjectile.SetTarget(target);
        newProjectile.SetDamage(damage);
    }
}