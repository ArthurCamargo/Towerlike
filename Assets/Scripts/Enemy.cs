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
        if(effects.Exists(e => e.effect == Attributes.Effects.CURSE)) {
            elementalDamage *= 1.5f;
        }
        health -= elementalDamage;

        Debug.Log("Attack " + elementalDamage + ", " + attack.element);

        if(health <= 0 && !dead) {
            Drop();
            Die();
            return;
        }

        foreach(Effect effect in attack.effects) {
            if(effect.TryEffectProc()) {
                effects.Add(new Effect(effect));
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
            case Attributes.Effects.BURN:
                if(effect.Tick()) {
                    this.TakeAttack(new Attack(effect.damage, Attributes.Elements.FIRE));
                    this.transform.GetComponent<Renderer>().material.color = Color.red + Color.yellow;
                }
                break;

            case Attributes.Effects.BLEED:
                if(effect.Tick()) {
                    this.TakeAttack(new Attack(effect.damage, Attributes.Elements.NONE));
                    this.transform.GetComponent<Renderer>().material.color = Color.red;
                }
                break;

            case Attributes.Effects.SLOW:
                if(!effect.isOn) {
                    effect.isOn = true;
                    this.transform.GetComponent<Renderer>().material.color = Color.blue;
                    NavMeshAgent navMeshAgent = this.GetComponent<NavMeshAgent>();
                    effect.damage = navMeshAgent.speed * (effect.damagePercent / 100);
                    this.GetComponent<NavMeshAgent>().speed -= effect.damage;
                }
                break;

            case Attributes.Effects.POISON:
                if(effect.Tick()) {
                    this.TakeAttack(new Attack(effect.damage, Attributes.Elements.PLANT));
                    this.transform.GetComponent<Renderer>().material.color = Color.green;
                }
                break;

            case Attributes.Effects.STUN:
                if(!pathfinder.isStopped) {
                    pathfinder.isStopped = true;
                    this.transform.GetComponent<Renderer>().material.color = Color.black;
                }
                break;

            case Attributes.Effects.FEAR:
                if(!effect.isOn) {
                    effect.isOn = true;
                    this.transform.GetComponent<Renderer>().material.color = Color.cyan;
                    pathfinder.SetDestination(GameObject.FindGameObjectWithTag("EnemySpawner").transform.position);
                }
                break;

            case Attributes.Effects.KNOCKBACK:
                if(!effect.isOn) {
                    effect.isOn= true;
                } 
                break;

            case Attributes.Effects.CURSE:
                if(!effect.isOn) {
                    effect.isOn = true;
                    this.transform.GetComponent<Renderer>().material.color = Color.magenta;
                }
                break;
        }
    }

    private void ApplyEffectEnd(Effect effect) {
        switch(effect.effect) {
            case Attributes.Effects.NONE:
                break;

            case Attributes.Effects.BURN:
                break;

            case Attributes.Effects.BLEED:
                break;

            case Attributes.Effects.SLOW:
                this.GetComponent<NavMeshAgent>().speed += effect.damage;
                break;

            case Attributes.Effects.POISON:
                break;

            case Attributes.Effects.STUN:
                pathfinder.isStopped = false;
                break;

            case Attributes.Effects.FEAR:
                pathfinder.SetDestination(target.transform.position);
                break;

            case Attributes.Effects.KNOCKBACK:
                break;

            case Attributes.Effects.CURSE:
                break;
        }
        this.transform.GetComponent<Renderer>().material.color = Color.gray;
    }
}
