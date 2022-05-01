using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingState : PlayerState {
    PlayerState PlayerState.doAction(Player player) {
        Collider hitObject = null;
        Node node = null;
        hitObject = player.SelectObjectWithTag("Obstacle");

        if(hitObject != null) {
            node = hitObject.GetComponent<Node>();

            if(!node.hasTower) {
                node.hasTower = true;
                player.PlaceTower(player.defautTower, hitObject.transform);
                Inventory.instance.Remove(player.holdingItem);
                return player.freeState;
            }
            return player.buildingState;
        }
        else {
            return player.buildingState;
        }
    }
}
