using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Tower : MonoBehaviour
{
    public Transform towerPrefab;
    public Transform attackPlaceHolder;

    public Transform target;
    public string enemyTag = "Enemy";
    public Projectile projectile;
    public float msBetweenAttacks = 1000;
    public float attackSpeed = 5;
    public float damage = 1.0f;
    public float range = 5;
    public int socketNumber = 4;
    public List<SocketItem> equipedItems;
    private float nextAttackTime;

    protected virtual void Start() {
        attackPlaceHolder = towerPrefab.Find("Crystal").transform;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    protected virtual void Update() {
       if(target != null) {
            if (Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + msBetweenAttacks / 1000;
                Attack();
            }
        }
    }

    //See if there is an enemy at sight
    public void Explode(Vector3 pos) {
        Collider[] colliders = Physics.OverlapSphere(pos, range);
        foreach(Collider collider in colliders) {
            if (collider.tag == "Enemy") {
                collider.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }

    //See if there is an enemy at sight
    public void UpdateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

        }

        if (nearestEnemy != null && shortestDistance < range) {
            target = nearestEnemy.transform;
        }
        else {
            target = null;
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, range);
    }
    
    void UpdateAttributes() {
    }
    
    public void EquipItem(SocketItem item) {
        int itemIndex;
        itemIndex = equipedItems.FindIndex(i => i.name == item.name);
        if(itemIndex != -1) {
            equipedItems[itemIndex].level ++;
        }
        else {
            equipedItems.Add(item);
        }
        UpdateAttributes();
    }
    
    public void UnequipItem(SocketItem item) {
        equipedItems.Remove(item);
        UpdateAttributes();
    }

    public abstract void Attack();
}