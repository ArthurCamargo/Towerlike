using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingCombatState : PlayerState {
    PlayerState PlayerState.doAction(Player player) {
        Collider hitObject = null;
        Tower tower = null;
        hitObject = player.SelectObjectWithTag("Tower");

        if(hitObject != null) {
            tower = hitObject.GetComponentInParent<Tower>();
            player.ChangeTowerCombat(tower);

            return player.freeState;
        }
        else {
            return player.changingCombatState;
        }
    }
}
