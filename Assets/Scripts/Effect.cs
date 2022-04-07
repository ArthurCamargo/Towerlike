using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effect {
    public Attributes.Effects effect;
    public float duration;
    public float tickFrequency;
    public float damage;
    public float procChance;

    private float nextTick;
    private float endOfDuration;

    public Effect(Attributes.Effects effect, float duration, float tickFrequency, float damage, float procChance) {
        this.effect = effect;
        this.duration = duration;
        this.tickFrequency = tickFrequency;
        this.damage = damage;
        this.procChance = procChance;
    }

    public Effect() {
        this.effect = Attributes.Effects.NONE;
        this.duration = 0;
        this.tickFrequency = 0;
        this.damage = 0;
        this.procChance = 0;
    }

    internal void Merge(Effect effect) {
        this.duration = Math.Max(this.duration, effect.duration);
        this.tickFrequency += Math.Min(this.tickFrequency, effect.tickFrequency);
        this.damage += Math.Max(this.damage, effect.damage);
        this.procChance += effect.procChance;
    }

    public bool TryEffectProc() {
        if(Utility.ProcTest(this.procChance)) {
            Debug.Log("Proc " + effect);
            this.StartEffect();
            return true;
        }
        else {
            Debug.Log("Miss " + effect);
            return false;
        }
    }

    private void StartEffect() {
        nextTick = Time.time;
        endOfDuration = Time.time + this.duration;
    }

    public bool ExpiredDuration() {
        if(endOfDuration <= Time.time) {
            return true;
        }
        else {
            return false;
        }
    }

    public bool Tick() {
        if(nextTick <= Time.time) {
            nextTick = Time.time + tickFrequency;
            return true;
        }
        else {
            return false;
        }
    }
}
