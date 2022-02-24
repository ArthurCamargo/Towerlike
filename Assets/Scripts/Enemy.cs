using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum State {Idle, Chasing};
    State currentState;

    NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;

    float attackDistanceThreshold = 1f;
    float damage = 1;

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
        if (!hasTarget)
            return;

        targetEntity.OnDeath += OnTargetDeath;
        
        float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

        if(sqrDstToTarget < Mathf.Pow(attackDistanceThreshold, 2)) {
            OnBaseHit();
        }
    }

    void OnBaseHit() {
        targetEntity.TakeDamage(damage);
        gameObject.GetComponent<LivingEntity>().BaseHit();
    }

    void OnTargetDeath()
    {
        hasTarget = false;
    }
}
