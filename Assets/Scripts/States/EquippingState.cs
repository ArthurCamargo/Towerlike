using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippingState : State
{
    State State.doAction(Player p)
    {
        Collider hitObject = null;
        Tower t = null;
        hitObject = p.SelectObjectWithTag("Tower");

        if(hitObject != null)
        {
            t = hitObject.GetComponentInParent<Tower>();
            t.EquipItem(p.currentItem);
            Inventory.instance.Remove(p.currentItem);
            return p.movingState;
        }
        else {
            return p.equippingState;
        }
    }
}
