using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Socket Item", menuName = "Inventory/Socket Item")]
public class SocketItem : Item
{
    public int level;
    public override void Use()
    {
        base.Use();
        Player.instance.holdingItem = this;
        Player.instance.currentState = Player.instance.equippingItemState;
    }
}
