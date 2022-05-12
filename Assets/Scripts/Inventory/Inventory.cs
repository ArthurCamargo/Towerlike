using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    #region Singleton
    public static Inventory instance;

    void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
        //allItems = Resources.LoadAll<Item>("Items");
        combatItems = Resources.LoadAll<Item>("Items/Combat Items");
        towerItems = Resources.LoadAll<Item>("Items/Tower Items");
        //socketItems = Resources.LoadAll<Item>("Items/Socket Items");
        elemenItems = Resources.LoadAll<Item>("Items/Socket Items/Element");
        effectItems = Resources.LoadAll<Item>("Items/Socket Items/Effect");
        statItems = Resources.LoadAll<Item>("Items/Socket Items/Stat");
        foreach(Item i in defaultItems) {
            this.Add(Instantiate(i));
        }
        
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 20;

    public List<Item> defaultItems;

    public List<Item> items = new List<Item>();

    private Item[] allItems, combatItems, towerItems, socketItems, elemenItems, effectItems, statItems;
    private System.Random rand = new System.Random();

    public void TryDrop(List<Item> dropList, float enemyDropChance) {
        if(dropList == null)
            return;
        if(dropList.Count == 0)
            return;

        double chance = rand.NextDouble();
        int itemIdx = Random.Range(0, dropList.Count);
        if(chance < enemyDropChance / 100.0) {       // 40% chance de combat item
            this.Add(Instantiate(dropList[itemIdx]));
        }
    }

    public void TryDropRandom() {
        double chance = rand.NextDouble();
        if(chance < 20 / 100.0) {       // 40% chance de combat item
            AddRandom();
        }
    }

    public void AddRandom() {
        double chance = rand.NextDouble();
        if(chance < 40 / 100.0) {       // 40% chance de combat item
            AddRandomCombatItem();
        } 
        else if(chance < 80 / 100.0) {  // 40% chance de socket item
            AddRandomSocketItem();
        }
        else {                          // 20% chance de tower item
            AddRandomTowerItem(); 
        }
    }

    public void AddRandomCombatItem() {
        int itemIdx = Random.Range(0, combatItems.Length);
        this.Add(Instantiate(combatItems[itemIdx]));
    }

    public void AddRandomTowerItem() {
        int itemIdx = Random.Range(0, towerItems.Length);
        this.Add(Instantiate(towerItems[itemIdx]));
    }

    public void AddRandomSocketItem() {
        double chance = rand.NextDouble();
        if(chance < 40 / 100.0) {       // 40% chance de combat item
            AddRandomElementItem();
        }
        else if(chance < 80 / 100.0) {  // 40% chance de socket item
            AddRandomStatItem();
        }
        else {                          // 20% chance de tower item
            AddRandomEffectItem();
        }
    }

    public void AddRandomElementItem() {
        int itemIdx = Random.Range(0, elemenItems.Length);
        this.Add(Instantiate(elemenItems[itemIdx]));
    }

    public void AddRandomEffectItem() {
        int itemIdx = Random.Range(0, effectItems.Length);
        this.Add(Instantiate(effectItems[itemIdx]));
    }

    public void AddRandomStatItem() {
        int itemIdx = Random.Range(0, statItems.Length);
        this.Add(Instantiate(statItems[itemIdx]));
    }

    public void Add(Item item) {
        if(items.Count >= space) {
            Debug.Log("Not enough room!");
            return;
        }

        items.Add(item);

        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }

    public void Remove(Item item) {
        items.Remove(item);

        if(onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
