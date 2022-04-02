using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Attributes
{
    public float damage;
    public float range;
    public float attackSpeed;
    public float projectileSpeed;
    public float sockets;
    public Elements element;
    public Effect effect;

    public enum Elements {
        NONE,
        FIRE,
        WATER,
        PLANT,
        LIGHT,
        DARKNESS,
    }

    public enum Effect {
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

    public Attributes(float damage, float range, float attackSpeed, float projectileSpeed, float sockets, Elements element, Effect effect) {
        this.damage = damage;
        this.range = range;
        this.attackSpeed = attackSpeed;
        this.projectileSpeed = projectileSpeed;
        this.sockets = sockets;
        this.element = element;
        this.effect = effect;
    }

    public Attributes(float damage, float range, float attackSpeed, float projectileSpeed, float sockets) {
        this.damage = damage;
        this.range = range;
        this.attackSpeed = attackSpeed;
        this.projectileSpeed = projectileSpeed;
        this.sockets = sockets;
        this.element = Elements.NONE;
        this.effect = Effect.NONE;
    }

    public void DeepCopy(Attributes original) {
        this.damage = original.damage;
        this.range = original.range;
        this.attackSpeed = original.attackSpeed;
        this.projectileSpeed = original.projectileSpeed;
        this.sockets = original.sockets;
        this.element = original.element;
        this.effect = original.effect;
    }

}
