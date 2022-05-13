using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LaserTower : Tower {
    public Transform attackPrefab;

    private Transform attackLaser;
    public float laserAttackSpeed;
    private bool laserIsOn;

    protected override void Start() {
        laserIsOn = false;
    }

    protected override void Update() {
        if(target == null) {
            if(laserIsOn) {
                laserIsOn = false;
                FindObjectOfType<AudioManager>().Play("Laser End");
                Destroy(attackLaser.gameObject);
            }
            if(UpdateTarget()) {
                laserAttackSpeed = attributes.attackSpeed;
                FindObjectOfType<AudioManager>().Play("Laser Start");
                attackLaser = Instantiate(attackPrefab, attackPlaceHolder.transform.position, Quaternion.identity) as Transform;
                laserIsOn = true;
            }
        }
        else {
            if(attackLaser)
            {
                Vector3 laserLocalScale = attackLaser.transform.localScale;
                float distanceToTarget = Vector3.Distance(attackPlaceHolder.position, target.position);

                if(distanceToTarget > attributes.range) {
                    target = null;
                    return;
                }

                attackLaser.transform.LookAt(target);  
                attackLaser.transform.localScale = new Vector3(laserLocalScale.x, distanceToTarget / 2, laserLocalScale.z);
                attackLaser.transform.position = attackPlaceHolder.position + (target.position - attackPlaceHolder.position)/2;
                attackLaser.transform.Rotate(90, 0, 0);

                if(Time.time > nextAttackTime) {
                    nextAttackTime = Time.time + 1 / laserAttackSpeed;
                    laserAttackSpeed += attributes.attackSpeed;
                    Attack();
                }
            }
        }
    }

    public override void Attack() {
        if(target)
            Laser(target);
    }

    public void Laser(Transform target) {
        target.GetComponent<Enemy>().TakeAttack(new Attack(attributes.damage, attributes.element, attributes.effects));
    }

    public override void BeforeDestroy() {
        if(laserIsOn) {
            laserIsOn = false;
            FindObjectOfType<AudioManager>().Play("Laser End");
            Destroy(attackLaser.gameObject);
        }
    }
}