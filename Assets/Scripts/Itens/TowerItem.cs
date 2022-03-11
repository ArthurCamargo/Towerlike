using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower Item", menuName = "Inventory/Tower Item")]
public class TowerItem : Item
{
    public override void Use()
    {
        base.Use();
        Player.instance.StartSelectObstacle(this);

    }

}
