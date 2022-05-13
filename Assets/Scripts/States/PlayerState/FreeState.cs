using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeState : PlayerState {
    PlayerState PlayerState.doAction(Player player) {
        if(Input.GetButtonDown("TooglePause")) {
            player.speedController.TooglePause();
        }
        
        if(Input.GetButtonDown("ToogleSpeed")) {
            player.speedController.Toogle3Times();
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