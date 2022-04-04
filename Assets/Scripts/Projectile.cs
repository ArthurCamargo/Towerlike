using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;
    float speed;
    Attack attack;
    Transform target;
    LivingEntity targetEntity;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetAttack(Attack newAttack)
    {
        attack = newAttack;
    }
    private void Start()
    {
        if(target == null)
            GameObject.Destroy(gameObject);
        else {
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;
        }

    }

    // Update is called once per frame
    void Update()
    {
        float moveDistance = speed * Time.deltaTime;

        CheckCollisions(moveDistance);
        // Homing shot
        if(target != null)
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveDistance);
    }

    void CheckCollisions(float moveDistance) {
        if (gameObject == null || target == null)
            return;
        Ray ray = new Ray(transform.position, (target.position - transform.position).normalized);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide)) {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit) {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if(damageableObject != null) {
            damageableObject.TakeHit(attack, hit);
        }
    }

    void OnTargetDeath() {
        if(gameObject != null) {
            GameObject.Destroy(gameObject);
        }
    }
}   


