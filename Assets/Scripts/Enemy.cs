using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

    NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;

    float attackDistanceThreshold = 1f;
    public float enemyDamage = 1;
    public Attributes.Elements enemyElement;
    public List<Effect> effects;

    bool hasTarget;


    protected override void Start() {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();
        effects = new List<Effect>();

        LockOnTargetByTag("Base");
    }

    void Update() {
        if(!hasTarget) {
            return;
        }
        targetEntity.OnDeath += OnTargetDeath;
        if(CheckTowerHit()) {
            return;
        }

        UpdateEffects(effects);
    }

    void OnBaseHit() {
        targetEntity.TakeAttack(new Attack(enemyDamage));
        gameObject.GetComponent<LivingEntity>().BaseHit();
    }

    void OnTargetDeath() {
        hasTarget = false;
    }

    private bool CheckTowerHit() {
        if(!hasTarget)
            return false;

        float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

        if(sqrDstToTarget < Mathf.Pow(attackDistanceThreshold, 2)) {
            OnBaseHit();
            return true;
        }
        else {
            return false;
        }
    }

    private void LockOnTargetByTag(string targetTag) {
        GameObject playerBase = GameObject.FindGameObjectWithTag(targetTag);

        if(playerBase != null) {
            hasTarget = true;

            target = playerBase.transform;
            pathfinder.SetDestination(target.position);
            targetEntity = target.GetComponent<LivingEntity>();
        }
    }

    public override void TakeAttack(Attack attack) {
        float elementalDamage = 0;

        Debug.Log("Attack " + attack.damage + ", " + attack.element + ", " + attack.effects.ToString());

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

        foreach(Effect effect in attack.effects) {
            if(effect.TryEffectProc()) {
                effects.Add(effect);
            }
        }
    }

    private void UpdateEffects(List<Effect> effects) {
        List<int> effectsIndexToRemove = new List<int>();
        int index = 0;

        foreach(Effect effect in effects) {
            if(effect.ExpiredDuration()) {
                ApplyEffectEnd(effect);
                effectsIndexToRemove.Add(index);
            }
            else {
                ApplyEffect(effect);
            }
            index++;
        }
        foreach(int effectIndex in effectsIndexToRemove) {
            effects.RemoveAt(effectIndex);
        }
    }

    private void ApplyEffect(Effect effect) {
        switch(effect.effect) {
            case Attributes.Effects.NONE:
                break;

            case Attributes.Effects.BURN:
                if(effect.Tick()) {
                    this.TakeAttack(new Attack(effect.damage, Attributes.Elements.FIRE));
                    this.transform.GetComponent<Renderer>().material.color = Color.blue;
                }
                break;

            case Attributes.Effects.BLEED:
                break;

            case Attributes.Effects.SLOW:
                break;

            case Attributes.Effects.POISON:
                break;

            case Attributes.Effects.STUN:
                pathfinder.isStopped = true;
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

    private void ApplyEffectEnd(Effect effect) {
        switch(effect.effect) {
            case Attributes.Effects.NONE:
                break;

            case Attributes.Effects.BURN:
                this.transform.GetComponent<Renderer>().material.color = Color.red;
                break;

            case Attributes.Effects.BLEED:
                break;

            case Attributes.Effects.SLOW:
                break;

            case Attributes.Effects.POISON:
                break;

            case Attributes.Effects.STUN:
                pathfinder.isStopped = false;
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
