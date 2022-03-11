using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PulseTower : Tower
{
    public Transform attackPrefab;
    private Transform attackSphere;
    private float pulseTime;
    private bool attacking = false;
    protected override void Start() {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (!attacking)
            return;

        if (Time.time > pulseTime)
        {
            pulseTime = Time.time + msBetweenShoots / 1000;
            Destroy(attackSphere.gameObject);
        }
    }

    public override void Attack() {
        Pulse();
    }

    public void Pulse() {
        attackSphere = Instantiate(attackPrefab, placeToShoot.transform.position, Quaternion.identity) as Transform;
        attacking = true;
        attackSphere.localScale = Vector3.one * range * 2;
        Explode(placeToShoot.transform.position);
        
    }


}