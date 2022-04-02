using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingClassState : State
{
    State State.doAction(Player p)
    {
        Collider hitObject = null;
        Tower t = null;
        hitObject = p.SelectObjectWithTag("Tower");

        if(hitObject != null)
        {
            t = hitObject.GetComponentInParent<Tower>();
            p.EvolveTower(t, p.currentItem);
            Inventory.instance.Remove(p.currentItem);
            return p.movingState;
        }
        else {
            return p.changingState;
        }
    }
}