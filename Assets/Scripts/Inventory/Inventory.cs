using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour 
{
    #region Singleton
    public static Inventory instance;

    void Awake() {
        if(instance != null) {
            Debug.LogWarning("More than one instance of Inventory found!");
            return;
        }
        instance = this;
        //allItems = Resources.LoadAll<Item>("../Resources/Items");
        //Debug.Log(allItems.Length);
    }

    #endregion

    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;

    public int space = 20;

    public List<Item> items = new List<Item>();

    public Item[] allItems;

    public void AddRandom()
    {
        int itemIdx = Random.Range(0, allItems.Length);
        this.Add(allItems[itemIdx]);
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

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }
}
