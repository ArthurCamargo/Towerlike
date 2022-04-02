using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PulseTower : Tower
{
    public Transform attackPrefab;
    private Transform attackSphere;
    private float pulseEndTime;
    private bool sphereOn = false;
    protected override void Start() {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (!sphereOn)
            return;

        if (Time.time > pulseEndTime)
        {
            Destroy(attackSphere.gameObject);
            sphereOn = false;
        }
    }

    public override void Attack() {
        Pulse();
    }

    public void Pulse() {
        //Create sphere as pulse animation
        attackSphere = Instantiate(attackPrefab, attackPlaceHolder.transform.position, Quaternion.identity) as Transform;
        attackSphere.localScale = Vector3.one * attributes.range * 2;
        sphereOn = true;
        pulseEndTime = Time.time + (1 / attributes.attackSpeed) / 2;

        //Do damage on enemies
        Explode(attackPlaceHolder.transform.position);
        
    }


}