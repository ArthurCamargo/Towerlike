using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attack {
    public float damage;
    public List<Attributes.Effects> effects;
    public Attributes.Elements element;

    public Attack(float damage, Attributes.Elements element, List<Attributes.Effects> effects) {
        this.damage = damage;
        this.element = element;
        this.effects = effects;   
    }

    public Attack(float damage, Attributes.Elements element) {
        this.damage = damage;
        this.element = element;
        this.effects = new List<Attributes.Effects>();
    }

    public Attack(float damage) {
        this.damage = damage;
        this.element = Attributes.Elements.NONE;
        this.effects = new List<Attributes.Effects>();
    }
}
