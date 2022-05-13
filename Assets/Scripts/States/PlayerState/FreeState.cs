using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeState : PlayerState {
    PlayerState PlayerState.doAction(Player player) {
        
        CheckTowerClick(player);

        return this;
    }

    PlayerState CheckTowerClick(Player player) {
        Collider hitObject = null;
        hitObject = player.SelectObjectWithTag("Tower");

        if(hitObject != null) {
            player.objectViewUI.gameObject.SetActive(true);
            player.objectViewUI.GatherTowerInformation(hitObject.gameObject.GetComponent<Transform>());
            return this;
        }
        return this;
    }

}