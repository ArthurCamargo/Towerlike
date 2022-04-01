using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeState : PlayerState {
    PlayerState PlayerState.doAction(Player player) {
        return this;
    }
}
