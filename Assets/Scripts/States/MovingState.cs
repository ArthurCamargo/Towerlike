using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState: State
{
    State State.doAction(Player p)
    {
        Debug.Log("Moving");
        return this;
    }
}
