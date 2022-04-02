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
    
    //See if there is an enemy at sight
    public void Explode(Vector3 pos)
    {
        Collider[] colliders = Physics.OverlapSphere(pos, range);
        foreach(Collider collider in colliders) {
            if (collider.tag == "Enemy") {
                collider.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }
    

    public void Pulse() {
        //Create sphere as pulse animation
        attackSphere = Instantiate(attackPrefab, attackPlaceHolder.transform.position, Quaternion.identity) as Transform;
        attackSphere.localScale = Vector3.one * range * 2;
        sphereOn = true;
        pulseEndTime = Time.time + (msBetweenAttacks / 1000)/2;

        //Do damage on enemies
        Explode(attackPlaceHolder.transform.position);
        
    }


}