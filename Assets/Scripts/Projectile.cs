using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask collisionMask;
    float speed = 2f;
    float damage = 1f;
    Transform target;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    // Update is called once per frame
    void Update()
    {
        float moveDistance = speed * Time.deltaTime;

        CheckCollisions(moveDistance);
        // Homing shot
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveDistance);
    }

    void CheckCollisions(float moveDistance) {
        Ray ray = new Ray(transform.position, (target.position - transform.position).normalized);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide)) {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit) {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if(damageableObject != null) {
            damageableObject.TakeHit(damage, hit);
        }
        GameObject.Destroy(gameObject);
    }
}   


