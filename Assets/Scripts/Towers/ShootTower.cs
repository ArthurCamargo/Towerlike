using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShootTower : Tower
{
    protected override void Start() {
        base.Start();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }
    
    //See if there is an enemy at sight
    public void UpdateTarget() {
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

    public override void Attack()
    {
        Shoot(target);
    }

    public void Shoot(Transform target) {
        
        Projectile newProjectile = Instantiate(projectile, placeToShoot.position, placeToShoot.rotation) as Projectile;
        newProjectile.SetSpeed(shootSpeed);
        newProjectile.SetTarget(target);
        newProjectile.SetDamage(damage);
    }
}