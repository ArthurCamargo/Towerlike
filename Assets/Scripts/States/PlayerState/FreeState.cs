using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeState : PlayerState {
    PlayerState PlayerState.doAction(Player player) {
        if(Input.GetButtonDown("TooglePlay"))
        {
            player.speedController.TooglePlay();
        }

        CheckObjectClick(player);

        return this;
    }

    PlayerState CheckObjectClick(Player player) {
        Collider hitObject = null;
        hitObject = player.SelectObjectWithTag("Tower");

        if(hitObject != null) {
            player.currentObject = hitObject.gameObject;
            Debug.Log(player.currentObject);
            player.objectViewUI.GatherInformation(player.currentObject);
            return this;
        }
        return this;
    }
        
}
