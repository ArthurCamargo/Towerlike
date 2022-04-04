using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack {
    public float damage;
    public Attributes.Effects effect;
    public Attributes.Elements element;

    public Attack(float damage, Attributes.Effects effect, Attributes.Elements element) {
        this.damage = damage;
        this.effect = effect;
        this.element = element;
    }

    public Attack(float damage) {
        this.damage = damage;
    }
}
