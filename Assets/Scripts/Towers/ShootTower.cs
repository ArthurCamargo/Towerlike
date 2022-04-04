using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShootTower : Tower {
    public Projectile projectile;

    protected override void Start() {
        base.Start();
    }

    public override void Attack() {
        Shoot(target);
    }

    public void Shoot(Transform target) {

        Projectile newProjectile = Instantiate(projectile, attackPlaceHolder.position, attackPlaceHolder.rotation) as Projectile;
        newProjectile.SetSpeed(attributes.projectileSpeed);
        newProjectile.SetTarget(target);
        newProjectile.SetAttack(new Attack(attributes.damage, attributes.effect, attributes.element));
    }
}