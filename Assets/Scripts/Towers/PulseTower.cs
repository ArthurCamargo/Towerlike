using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PulseTower : Tower
{
    protected override void Start() {
        base.Start();
    }

    public override void Attack() {
        Pulse();
    }

    public void Pulse() {
        Explode(placeToShoot.transform.position);
        
    }


}