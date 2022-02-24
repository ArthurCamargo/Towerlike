using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity
{
    NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;

    float attackDistanceThreshold = 1f;


    protected override void Start() {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Base").transform;
        pathfinder.SetDestination(target.position);
        targetEntity = target.GetComponent<LivingEntity>();
        targetEntity.OnDeath += OnTargetDeath;
    }

    void OnTargetDeath() {

    }

    void Update() {
        float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;

        if(sqrDstToTarget < Mathf.Pow(attackDistanceThreshold, 2)) {

        }
    }
}
