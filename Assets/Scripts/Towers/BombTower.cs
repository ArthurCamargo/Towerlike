using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BombTower : Tower {
    public BombProjectile bomb;

    private Vector3 fixedTarget;

    protected override void Start() {
    }

    protected override void Update() {
        if(target == null) {
            UpdateTarget();
        }
        else {
            float distanceToTarget = Vector3.Distance(attackPlaceHolder.position, target.position);
            if(distanceToTarget > attributes.range) {
                target = null;
                return;
            }
            if(Time.time > nextAttackTime) {
                nextAttackTime = Time.time + 1 / attributes.attackSpeed;
                fixedTarget = new Vector3(target.position.x, target.position.y, target.position.z);
                Attack();
            }
        }
    }

    public override void Attack() {
        Bomb(target);
    }

    public void Bomb(Transform target) {
        BombProjectile newBomb = Instantiate(bomb, attackPlaceHolder.position, attackPlaceHolder.rotation) as BombProjectile;
        newBomb.SetSpeed(attributes.projectileSpeed);
        newBomb.SetFixedTarget(target.position);
        newBomb.SetBombRange(attributes.range/2);
        newBomb.SetAttack(new Attack(attributes.damage, attributes.element, attributes.effects));
    }

    public override void BeforeDestroy() {
    }
}