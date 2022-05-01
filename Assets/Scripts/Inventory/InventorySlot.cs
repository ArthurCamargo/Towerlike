using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;

    Item item;

    TooltipTrigger tooltipTrigger;

    public void AddItem(Item newItem)
    {
        item = newItem;
        tooltipTrigger = icon.GetComponent<TooltipTrigger>();

        icon.sprite = item.icon;
        icon.enabled = true;
        tooltipTrigger.header = item.name;
        tooltipTrigger.body = item.description;

        removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
        }
    }

}
