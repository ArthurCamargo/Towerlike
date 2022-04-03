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
        allItems = Resources.LoadAll<Item>("Items");
        this.Add(Instantiate(defaultItem));
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 20;

    public Item defaultItem;

    public List<Item> items = new List<Item>();

    private Item[] allItems;

    public void AddRandom() {
        int itemIdx = Random.Range(0, allItems.Length);
        this.Add(Instantiate(allItems[itemIdx]));
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
