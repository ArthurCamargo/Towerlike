using UnityEngine;
using UnityEngine.EventSystems;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {
    new public string name = "New Item";

    public string description = "";

    public Sprite icon = null;

    public virtual void Use() {
        // Use the item
        Debug.Log("Using " + name);
        Debug.Log(description);
    }
}
