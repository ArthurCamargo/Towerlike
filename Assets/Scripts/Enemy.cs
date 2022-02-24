using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity
{
    NavMeshAgent pathfinder;
    Transform target;


    protected override void Start() {
        base.Start();
        pathfinder = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Base").transform;
        pathfinder.SetDestination(target.position);
    }

    void Update() { 
        
    }
}
