using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tower : MonoBehaviour
{
    public Transform towerPrefab;
    public Transform placeToShoot;
    public Transform Target;
    public Projectile projectile;
    public float msBetweenShoots = 100;
    public float shootSpeed = 5;
    public float damage = 1.0f;
    public void Shoot(Transform Target) {
        
        Projectile newProjectile = Instantiate(projectile, placeToShoot.position, placeToShoot.rotation) as Projectile;
        newProjectile.SetSpeed(shootSpeed);
        newProjectile.SetTarget(Target);
    }
    public void SetDamage(float newDamage) {
        damage = newDamage;
    }
}