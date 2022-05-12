using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombProjectile : Projectile
{
    Vector3 fixedTarget;
    public float bombRange;
    public Transform animationModel;

    protected override void Start() {
        FindObjectOfType<AudioManager>().Play("Bomb Start");
    }

    protected override void Update() {

        float moveDistance = speed * Time.deltaTime;

        if(CheckTargetHit(moveDistance)) {
            Utility.Explode(fixedTarget, bombRange, attack.damage, attack.element, attack.effects);
            FindObjectOfType<AudioManager>().Play("Bomb Explosion");
            var animation = new GameObject().AddComponent<Animation>();
            animation.SetAnimation(animationModel, transform.position, 1, bombRange);
            GameObject.Destroy(gameObject);
        }
        else {
            transform.position = Vector3.MoveTowards(transform.position, fixedTarget, moveDistance);
        }
        
    }

    private bool CheckTargetHit(float moveDistance) {
        return Vector3.Distance(transform.position, fixedTarget) < moveDistance;
        
    }

    internal void SetFixedTarget(Vector3 position) {
        fixedTarget = position;
    }

    internal void SetBombRange(float bombRange) {
        this.bombRange = bombRange;
    }
}
