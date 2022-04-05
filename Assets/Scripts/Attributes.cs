using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attributes {
    public float damage;
    public float range;
    public float attackSpeed;
    public float projectileSpeed;
    public int sockets;
    public Elements element;
    public List<Effects> effects;

    public enum Elements {
        NONE,
        FIRE,
        WATER,
        PLANT,
        LIGHT,
        DARKNESS,
    }

    public enum Effects {
        NONE,
        BURN,
        BLEED,
        SLOW,
        POISON,
        STUN,
        FEAR,
        KNOCKBACK,
        HEAL,
        CURSE,//damage amplified
    }

    public Attributes() {
        this.damage = 0;
        this.range = 0;
        this.attackSpeed = 0;
        this.projectileSpeed = 0;
        this.sockets = 0;
        this.element = Elements.NONE;
        this.effects = new List<Effects>();
    }

    public Attributes(float damage, float range, float attackSpeed, float projectileSpeed, int sockets, Elements element, List<Effects> effects) {
        this.damage = damage;
        this.range = range;
        this.attackSpeed = attackSpeed;
        this.projectileSpeed = projectileSpeed;
        this.sockets = sockets;
        this.element = element;
        this.effects = effects;
    }

    public Attributes(float damage, float range, float attackSpeed, float projectileSpeed, int sockets) {
        this.damage = damage;
        this.range = range;
        this.attackSpeed = attackSpeed;
        this.projectileSpeed = projectileSpeed;
        this.sockets = sockets;
        this.element = Elements.NONE;
        this.effects = new List<Effects>();
    }

    public Attributes(Attributes attributes) {
        this.damage = attributes.damage;
        this.range = attributes.range;
        this.attackSpeed = attributes.attackSpeed;
        this.projectileSpeed = attributes.projectileSpeed;
        this.sockets = attributes.sockets;
        this.element = attributes.element;
        this.effects = attributes.effects;
    }

    public void DeepCopy(Attributes original) {
        this.damage = original.damage;
        this.range = original.range;
        this.attackSpeed = original.attackSpeed;
        this.projectileSpeed = original.projectileSpeed;
        this.sockets = original.sockets;
        this.element = original.element;
        this.effects = original.effects;
    }

    internal void Add(Attributes attributes) {
        this.damage += attributes.damage;
        this.range += attributes.range;
        this.attackSpeed += attributes.attackSpeed;
        this.projectileSpeed += attributes.projectileSpeed;
        this.sockets += attributes.sockets;
        this.element = attributes.element;
        this.effects = attributes.effects;
    }

    internal void Multiply(Attributes multipliers) {
        this.damage *= multipliers.damage;
        this.range *= multipliers.range;
        this.attackSpeed *= multipliers.attackSpeed;
        this.projectileSpeed *= multipliers.projectileSpeed;
    }


}
