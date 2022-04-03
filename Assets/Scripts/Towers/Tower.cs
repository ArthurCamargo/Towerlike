using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Tower : MonoBehaviour {
    public Transform towerPrefab;
    public Transform attackPlaceHolder;
    public Transform target;

    public string enemyTag = "Enemy";

    public List<SocketItem> equipedItems;

    public Attributes baseAttributes;
    public Attributes attributesMultipliers;
    public Attributes attributes;

    private float nextAttackTime;

    protected virtual void Awake() {
        //baseAttributes = new Attributes(1, 5, 1, 10, 2, Attributes.Elements.NONE, Attributes.Effect.NONE);
        //attributesMultipliers = new Attributes(1, 1, 1, 1, 1);
        attributes.DeepCopy(baseAttributes);
        attackPlaceHolder = towerPrefab.Find("Crystal").transform;
    }

    protected virtual void Start() {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }


    protected virtual void Update() {
        if(target != null) {
            if(Time.time > nextAttackTime) {
                nextAttackTime = Time.time + 1 / attributes.attackSpeed;
                Attack();
            }
        }
    }

    //See if there is an enemy at sight
    public void Explode(Vector3 pos) {
        Collider[] colliders = Physics.OverlapSphere(pos, attributes.range);
        foreach(Collider collider in colliders) {
            if(collider.tag == "Enemy") {
                collider.GetComponent<Enemy>().TakeDamage(attributes.damage);
            }
        }
    }

    //See if there is an enemy at sight
    public void UpdateTarget() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies) {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance) {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

        }

        if(nearestEnemy != null && shortestDistance < attributes.range) {
            target = nearestEnemy.transform;
        }
        else {
            target = null;
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, attributes.range);
    }

    public void UpdateAttributes() {
        Attributes itemsAttributes = new Attributes();

        foreach(SocketItem item in equipedItems) {
            switch(item.name) {
                // Basic Attributes
                case "Attack Speed":
                    itemsAttributes.attackSpeed += item.level * 0.2f;
                    break;

                case "Damage":
                    itemsAttributes.damage += item.level;
                    break;

                case "Range":
                    itemsAttributes.range += item.level;
                    break;
                // Elements
                case "Fire":
                    if(itemsAttributes.element == Attributes.Elements.NONE)
                        itemsAttributes.element = Attributes.Elements.FIRE;
                    break;

                case "Plant":
                    if(itemsAttributes.element == Attributes.Elements.NONE)
                        itemsAttributes.element = Attributes.Elements.PLANT;
                    break;

                case "Water":
                    if(itemsAttributes.element == Attributes.Elements.NONE)
                        itemsAttributes.element = Attributes.Elements.WATER;
                    break;

                case "Darkness":
                    if(itemsAttributes.element == Attributes.Elements.NONE)
                        itemsAttributes.element = Attributes.Elements.DARKNESS;
                    break;

                case "Light":
                    if(itemsAttributes.element == Attributes.Elements.NONE)
                        itemsAttributes.element = Attributes.Elements.LIGHT;
                    break;

                // Effects
                case "Bleed":
                    if(itemsAttributes.effect == Attributes.Effect.NONE)
                        itemsAttributes.effect = Attributes.Effect.BLEED;
                    break;

                case "Burn":
                    if(itemsAttributes.effect == Attributes.Effect.NONE)
                        itemsAttributes.effect = Attributes.Effect.BURN;
                    break;

                case "Curse":
                    if(itemsAttributes.effect == Attributes.Effect.NONE)
                        itemsAttributes.effect = Attributes.Effect.CURSE;
                    break;

                case "Poison":
                    if(itemsAttributes.effect == Attributes.Effect.NONE)
                        itemsAttributes.effect = Attributes.Effect.POISON;
                    break;

                case "Slow":
                    if(itemsAttributes.effect == Attributes.Effect.NONE)
                        itemsAttributes.effect = Attributes.Effect.SLOW;
                    break;
            }
        }
        attributes.DeepCopy(baseAttributes);
        attributes.Add(itemsAttributes);
        attributes.Multiply(attributesMultipliers);

    }

    public void EquipItem(SocketItem item) {
        int itemIndex;
        itemIndex = equipedItems.FindIndex(i => i.name == item.name);
        if(itemIndex != -1) {
            equipedItems[itemIndex].level++;
        }
        else if(equipedItems.Count < attributes.sockets) {
            equipedItems.Add(item);
        }
        else {
            Debug.Log("Torre sem Sockets disponíveis");
            return;
        }
        Inventory.instance.Remove(item);
        UpdateAttributes();
    }

    public void UnequipItem(SocketItem item) {
        equipedItems.Remove(item);
        UpdateAttributes();
    }

    public void TransferTowerStats(Tower oldTower) {
        equipedItems = new List<SocketItem>(oldTower.equipedItems);
        attributes.sockets = oldTower.attributes.sockets;
        UpdateAttributes();
    }

    public abstract void Attack();
}