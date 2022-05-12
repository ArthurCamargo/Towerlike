using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using static Utility;
using Random = UnityEngine.Random;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

    NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;
    Animator animator;
    float attackDistanceThreshold = 1f;
    public float enemyDamage = 1;
    public Attributes.Elements enemyElement;
    public string enemyTypeName;
    public List<Effect> effects;

    bool hasTarget;

    private void Awake() {
        pathfinder = GetComponent<NavMeshAgent>();
        effects = new List<Effect>();
        animator = GetComponent<Animator>();

        LockOnTargetByTag("Base");
    }
    protected override void Start() {
        base.Start();
        
    }

    void Update() {
        if(!hasTarget) {
            animator.SetBool("Walking", false);
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

    internal static Enemy GenerateEnemyFromWave(Wave currentWave, Vector3 spawnerPosition) {
        EnemyType randomEnemyType = currentWave.enemyTypes[Random.Range(0, currentWave.enemyTypes.Count)];

        Enemy spawnedEnemy = Instantiate(randomEnemyType.prefab, spawnerPosition, Quaternion.identity);
        spawnedEnemy.AdaptToEnemyType(randomEnemyType);
        spawnedEnemy.AdaptToWaveType(currentWave);
        spawnedEnemy.AdaptToWaveNumber(currentWave.waveNumber);
        spawnedEnemy.enemyTypeName = randomEnemyType.name;

       /*
        Color enemyColor = Color.gray;


        switch(spawnedEnemy.enemyElement) {
            case Attributes.Elements.NONE:
                enemyColor = Color.gray;
                break;
            case Attributes.Elements.FIRE:
                enemyColor = Color.red;
                break;
            case Attributes.Elements.WATER:
                enemyColor = Color.blue;
                break;
            case Attributes.Elements.PLANT:
                enemyColor = Color.green;
                break;
            case Attributes.Elements.LIGHT:
                enemyColor = Color.white;
                break;
            case Attributes.Elements.DARKNESS:
                enemyColor = Color.magenta;
                break;
        }

        spawnedEnemy.GetComponent<Renderer>().material.color = enemyColor;
       */

        return spawnedEnemy;
    }

    private void AdaptToWaveNumber(int waveNumber) {
        NavMeshAgent enemyNav = this.GetComponent<NavMeshAgent>();

        this.startingHealth = DifficultyIncreaseFunction(waveNumber, this.startingHealth, Wave.WAVE_DIFFICULTY_MULTIPLYER);
        enemyNav.speed = DifficultyIncreaseFunction(waveNumber, enemyNav.speed, Wave.WAVE_DIFFICULTY_MULTIPLYER);
        this.enemyDamage = DifficultyIncreaseFunction(waveNumber, this.enemyDamage, Wave.WAVE_DIFFICULTY_MULTIPLYER);
    }

    private void AdaptToWaveType(Wave currentWave) {
        NavMeshAgent enemyNav = this.GetComponent<NavMeshAgent>();

        this.startingHealth = AdaptToDifficulty(currentWave.enemyHealthType, this.startingHealth);
        enemyNav.speed = AdaptToDifficulty(currentWave.enemySpeedType, enemyNav.speed);
        this.enemyDamage = AdaptToDifficulty(currentWave.enemyDamageType, this.enemyDamage);
    }

    private void AdaptToEnemyType(EnemyType randomEnemyType) {

        // VIDA
        switch(randomEnemyType.healthType) {
            case Difficulty.VERY_LOW:
                this.startingHealth = Random.Range(1, 6);
                break;
            case Difficulty.LOW:
                this.startingHealth = Random.Range(6, 11);
                break;
            case Difficulty.MEDIUM:
                this.startingHealth = Random.Range(11, 21);
                break;
            case Difficulty.HIGH:
                this.startingHealth = Random.Range(21, 41);
                break;
            case Difficulty.VER_HIGH:
                this.startingHealth = Random.Range(41, 61);
                break;
        }


        // VELOCIDADE
        NavMeshAgent enemyNav = this.GetComponent<NavMeshAgent>(); 

        switch(randomEnemyType.speedType) {
            case Difficulty.VERY_LOW:
                enemyNav.speed = 1;
                break;
            case Difficulty.LOW:
                enemyNav.speed = 2;
                break;
            case Difficulty.MEDIUM:
                enemyNav.speed = 3;
                break;
            case Difficulty.HIGH:
                enemyNav.speed = 4;
                break;
            case Difficulty.VER_HIGH:
                enemyNav.speed = 5;
                break;
        }

        // DANO
        switch(randomEnemyType.damageType) {
            case Difficulty.VERY_LOW:
                this.enemyDamage = Random.Range(1, 6);
                break;
            case Difficulty.LOW:
                this.enemyDamage = Random.Range(6, 11);
                break;
            case Difficulty.MEDIUM:
                this.enemyDamage = Random.Range(11, 16);
                break;
            case Difficulty.HIGH:
                this.enemyDamage = Random.Range(16, 21);
                break;
            case Difficulty.VER_HIGH:
                this.enemyDamage = Random.Range(30, 51);
                break;
        }

        this.enemyElement = randomEnemyType.element;
    }

    private void LockOnTargetByTag(string targetTag) {
        GameObject playerBase = GameObject.FindGameObjectWithTag(targetTag);

        if(playerBase != null) {
            hasTarget = true;

            animator.SetBool("Walking", true);
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
        healthBarUI.fillAmount = health/startingHealth;

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
        List<Effect> effectsToRemove = new List<Effect>();

        foreach(Effect effect in effects) {
            if(effect.ExpiredDuration()) {
                ApplyEffectEnd(effect);
                effectsToRemove.Add(effect);
            }
            else {
                ApplyEffect(effect);
            }
        }
        foreach(Effect effect in effectsToRemove) {
            effects.Remove(effect);
        }
    }

    private void ApplyEffect(Effect effect) {
        switch(effect.effect) {
            case Attributes.Effects.BURN:
                if(effect.Tick()) {
                    this.TakeAttack(new Attack(effect.damage, Attributes.Elements.FIRE));
                    //this.transform.GetComponent<Renderer>().material.color = Color.red + Color.yellow;
                }
                break;

            case Attributes.Effects.BLEED:
                if(effect.Tick()) {
                    this.TakeAttack(new Attack(effect.damage, Attributes.Elements.NONE));
                    //this.transform.GetComponent<Renderer>().material.color = Color.red;
                }
                break;

            case Attributes.Effects.SLOW:
                if(!effect.isOn) {
                    effect.isOn = true;
                    //this.transform.GetComponent<Renderer>().material.color = Color.blue;
                    NavMeshAgent navMeshAgent = this.GetComponent<NavMeshAgent>();
                    effect.damage = navMeshAgent.speed * (effect.damagePercent / 100);
                    this.GetComponent<NavMeshAgent>().speed -= effect.damage;
                }
                break;

            case Attributes.Effects.POISON:
                if(effect.Tick()) {
                    this.TakeAttack(new Attack(effect.damage, Attributes.Elements.PLANT));
                    //this.transform.GetComponent<Renderer>().material.color = Color.green;
                }
                break;

            case Attributes.Effects.STUN:
                if(!pathfinder.isStopped) {
                    pathfinder.isStopped = true;
                    //this.transform.GetComponent<Renderer>().material.color = Color.black;
                }
                break;

            case Attributes.Effects.FEAR:
                if(!effect.isOn) {
                    effect.isOn = true;
                    //this.transform.GetComponent<Renderer>().material.color = Color.cyan;
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
                    //this.transform.GetComponent<Renderer>().material.color = Color.magenta;
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
        //this.transform.GetComponent<Renderer>().material.color = Color.gray;
    }
}
