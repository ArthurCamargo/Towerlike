using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Tower : MonoBehaviour
{
    public Transform towerPrefab;
    public Transform placeToShoot;

    public Transform target;
    public string enemyTag = "Enemy";
    public Projectile projectile;
    public float msBetweenShoots = 100;
    public float shootSpeed = 5;
    protected float nextShotTime;
    public float damage = 1.0f;
    public float range = 5;
    public int socketNumber = 4;
    public List<Item> equipedItems;


    protected virtual void Start() {

    }
    
    void Update() {
       if(target == null)
           return;
       

        if(Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShoots/ 1000;
            Attack();
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }
    
    void UpdateAttributes()
    {
        Debug.Log("Tu es mais forte agora");
    }
    
    public void EquipItem(Item item)
    {
        equipedItems.Add(item);
        UpdateAttributes();
    }
    
    public void UnequipItem(Item item)
    {
        equipedItems.Remove(item);
        UpdateAttributes();
    }

    public abstract void Attack();
}