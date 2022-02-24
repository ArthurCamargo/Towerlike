using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tower : MonoBehaviour
{
    public Transform towerPrefab;
    public Transform placeToShoot;
    public float range = 5;
    public Transform target;
    string enemyTag = "Enemy";
    public Projectile projectile;
    public float msBetweenShoots = 100;
    public float shootSpeed = 5;
    public float damage = 1.0f;

    
    float nextShotTime;
    
    void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    
    void Update() {
       if(target == null)
           return;
       

        if(Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShoots/ 1000;
            Shoot(target);
        }
    }
    //See if there is an enemy at sight
    public void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        
        foreach (GameObject enemy in enemies)
        {
            float distaceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distaceToEnemy < shortestDistance)
            {
                shortestDistance = distaceToEnemy;
                nearestEnemy = enemy;
            }

        }
        
        if (nearestEnemy != null && shortestDistance < range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void Shoot(Transform target) {
        
        Projectile newProjectile = Instantiate(projectile, placeToShoot.position, placeToShoot.rotation) as Projectile;
        newProjectile.SetSpeed(shootSpeed);
        newProjectile.SetTarget(target);
        newProjectile.SetDamage(damage);
    }
    public void SetDamage(float newDamage) {
        damage = newDamage;
    }
}