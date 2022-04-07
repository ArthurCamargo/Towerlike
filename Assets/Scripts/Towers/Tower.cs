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

    //Do damage to all enemies in a circle radius with center in pos
    public void Explode(Vector3 pos) {
        Collider[] colliders = Physics.OverlapSphere(pos, attributes.range);
        foreach(Collider collider in colliders) {
            if(collider.tag == "Enemy") {
                collider.GetComponent<Enemy>().TakeAttack(new Attack(attributes.damage, attributes.element, attributes.effects));
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
                    itemsAttributes.AddEffect(new Effect(Attributes.Effects.BLEED, 4, 1, 0.5f, 0, 50));
                    break;

                case "Burn":
                    itemsAttributes.AddEffect(new Effect(Attributes.Effects.BURN, 4, 1, 0.5f, 0, 50));
                    break;

                case "Curse":
                    itemsAttributes.AddEffect(new Effect(Attributes.Effects.CURSE, 4, 1, 0.5f, 0, 50));
                    break;

                case "Poison":
                    itemsAttributes.AddEffect(new Effect(Attributes.Effects.POISON, 4, 1, 0.5f, 0, 50));
                    break;

                case "Slow":
                    itemsAttributes.AddEffect(new Effect(Attributes.Effects.SLOW, 4, 1, 0, 20, 100));
                    break;

                case "Stun":
                    itemsAttributes.AddEffect(new Effect(Attributes.Effects.STUN, 2, 2, 0, 0, 50));
                    break;
            }
        }
        attributes.DeepCopy(baseAttributes);
        attributes.Add(itemsAttributes);
        attributes.Multiply(attributesMultipliers);

    }

    public void EquipItem(SocketItem item) {
        int itemIndex = equipedItems.FindIndex(i => i.name == item.name);

        if(itemIndex != -1) {
            equipedItems[itemIndex].level++;
        }
        else if(equipedItems.Count < attributes.sockets) {
            if(item.type == SocketItem.SocketType.ELEMENT) {
                if(attributes.element != Attributes.Elements.NONE) {
                    itemIndex = equipedItems.FindIndex(i => i.type == SocketItem.SocketType.ELEMENT);
                    equipedItems[itemIndex] = item;
                }
                else {
                    equipedItems.Add(item);
                }
            }
            else {
                equipedItems.Add(item);
            }
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