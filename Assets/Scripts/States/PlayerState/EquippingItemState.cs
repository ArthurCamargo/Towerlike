using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippingItemState : PlayerState {
    PlayerState PlayerState.doAction(Player player) {
        Collider hitObject = null;
        Tower tower = null;
        hitObject = player.SelectObjectWithTag("Tower");

        if(hitObject != null) {
            tower = hitObject.GetComponentInParent<Tower>();
            tower.EquipItem(player.holdingItem);
            Inventory.instance.Remove(player.holdingItem);
            
            return player.freeState;
        }
        else {
            return player.equippingItemState;
        }
    }
}
