using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingState : State
{
    State State.doAction(Player p)
    {
        Collider hitObject = null;
        Node n = null;
        hitObject = p.SelectObjectWithTag("Obstacle");

        if(hitObject != null)
        {
            n = hitObject.GetComponent<Node>();

            if(!n.hasTower) {
                n.hasTower = true;
                p.PlaceTower(p.defautTower, hitObject.transform);
                Inventory.instance.Remove(p.currentItem);
                return p.movingState;
            }
            return p.buildingState;
        }
        else {
            return p.buildingState;
        }
    }
}
