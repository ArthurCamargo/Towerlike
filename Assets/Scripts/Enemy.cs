using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {
    public enum State { Idle, Chasing };
    State currentState;

    NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;

    float attackDistanceThreshold = 1f;
    public float enemyDamage = 1;
    public Attributes.Elements enemyElement;

    bool hasTarget;


    protected override void Start() {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();

        currentState = State.Chasing;
        if(GameObject.FindGameObjectWithTag("Base").transform != null) {
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("Base").transform;
            pathfinder.SetDestination(target.position);
            targetEntity = target.GetComponent<LivingEntity>();
        }


    }

    void Update() {
        if(!hasTarget)
            return;

        targetEntity.OnDeath += OnTargetDeath;

        float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

        if(sqrDstToTarget < Mathf.Pow(attackDistanceThreshold, 2)) {
            OnBaseHit();
        }
    }

    void OnBaseHit() {
        targetEntity.TakeAttack(new Attack(enemyDamage));
        gameObject.GetComponent<LivingEntity>().BaseHit();
    }

    void OnTargetDeath() {
        hasTarget = false;
    }

    public override void TakeAttack(Attack attack) {
        float elementalDamage = 0;

        switch(attack.element) {
            case Attributes.Elements.NONE:
                elementalDamage = attack.damage;
                break; 
                
            case Attributes.Elements.FIRE: 
                if(enemyElement == Attributes.Elements.PLANT) {
                    elementalDamage = attack.damage * 2;
                }
                else {
                    elementalDamage = attack.damage;
                }
                break;

            case Attributes.Elements.WATER:
                if(enemyElement == Attributes.Elements.FIRE) {
                    elementalDamage = attack.damage * 2;
                }
                else {
                    elementalDamage = attack.damage;
                }
                break;

            case Attributes.Elements.PLANT:
                if(enemyElement == Attributes.Elements.WATER) {
                    elementalDamage = attack.damage * 2;
                }
                else {
                    elementalDamage = attack.damage;
                }
                break;

            case Attributes.Elements.LIGHT:
                if(enemyElement == Attributes.Elements.DARKNESS) {
                    elementalDamage = attack.damage * 2;
                }
                else {
                    elementalDamage = attack.damage;
                }
                break;

            case Attributes.Elements.DARKNESS:
                if(enemyElement == Attributes.Elements.LIGHT) {
                    elementalDamage = attack.damage * 2;
                }
                else {
                    elementalDamage = attack.damage;
                }
                break;
        }

        health -= elementalDamage;

        if(health <= 0 && !dead) {
            Drop();
            Die();
            return;
        }

        switch(attack.effect) {
            case Attributes.Effects.NONE:
                break;

            case Attributes.Effects.BURN:
                break;

            case Attributes.Effects.BLEED:
                break;

            case Attributes.Effects.SLOW:
                break;

            case Attributes.Effects.POISON:
                break;

            case Attributes.Effects.STUN:
                break;

            case Attributes.Effects.FEAR:
                break;

            case Attributes.Effects.KNOCKBACK:
                break;

            case Attributes.Effects.HEAL:
                break;

            case Attributes.Effects.CURSE:
                break;
        }

    }
}
