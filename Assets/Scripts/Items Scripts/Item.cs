using UnityEngine;
using UnityEngine.EventSystems;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    new public string name = "New Item";
    /*
    public enum itemType{
        Combat,
        Buff,
        Tower,
    };
    
    public itemType type = itemType.Combat;
    */
    
    public Sprite icon = null;

    public virtual void Use() {
        // Use the item
        Debug.Log("Using " + name);
    }
}
