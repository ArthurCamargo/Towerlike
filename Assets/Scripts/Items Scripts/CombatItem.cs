using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Item", menuName = "Inventory/Combat Item")]
public class CombatItem : Item
{
    public Transform towerPrefab;
    public override void Use()
    {
        base.Use();
        Player.instance.StartHoldingItem(this);
    }
}
