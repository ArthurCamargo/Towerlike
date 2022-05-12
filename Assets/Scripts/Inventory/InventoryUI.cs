using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Transform itemsParent;
    public GameObject inventoryUI;
    
    Inventory inventory;

    bool isLocked = false;

    InventorySlot[] slots;

    void Start()
    {
        inventory = Inventory.instance;
        inventory.onItemChangedCallback += UpdateUI;

        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
        UpdateUI();
    }


    public void SetLock(bool locking)
    {
        isLocked = locking;
    }


    public void Toogle()
    {
        if(!isLocked)
            inventoryUI.SetActive(!inventoryUI.activeSelf);
    }

    void Update()
    {
        if (Input.GetButtonDown("Inventory"))
            Toogle();
            
    }

    void UpdateUI()
    {
        for(int i = 0; i < slots.Length; i++){
            if(i < inventory.items.Count) { 
                slots[i].AddItem(inventory.items[i]);
            }
            else { 
                slots[i].ClearSlot();
            }
        }   
    }
}
