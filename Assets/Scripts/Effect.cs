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
    public float damagePercent;
    public float procChance;
    public bool isOn;

    private float nextTick;
    private float endOfDuration;

    public Effect(Attributes.Effects effect, float duration, float tickFrequency, float damage, float damagePercent, float procChance) {
        this.effect = effect;
        this.duration = duration;
        this.tickFrequency = tickFrequency;
        this.damage = damage;
        this.damagePercent = damagePercent;
        this.procChance = procChance;
        this.isOn = false;
    }

    public Effect() {
        this.effect = Attributes.Effects.NONE;
        this.duration = 0;
        this.tickFrequency = 0;
        this.damage = 0;
        this.damagePercent = 0;
        this.procChance = 0;
        this.isOn = false;
    }

    public Effect(Effect effect) {
        this.effect = effect.effect;
        this.duration = effect.duration;
        this.tickFrequency = effect.tickFrequency;
        this.damage = effect.damage;
        this.damagePercent = effect.damagePercent;
        this.procChance = effect.procChance;
        this.isOn = effect.isOn;
        this.nextTick = effect.nextTick;
        this.endOfDuration = effect.endOfDuration;
    }

    internal void Merge(Effect effect) {
        this.duration = Math.Max(this.duration, effect.duration);
        this.tickFrequency += Math.Min(this.tickFrequency, effect.tickFrequency);
        this.damage += Math.Max(this.damage, effect.damage);
        this.damagePercent = Math.Max(this.damagePercent, effect.damagePercent);
        this.procChance += effect.procChance;
    }

    internal void DeepCopy(Effect effect) {
        this.effect = effect.effect;
        this.duration = effect.duration;
        this.tickFrequency = effect.tickFrequency;
        this.damage = effect.damage;
        this.damagePercent = effect.damagePercent;
        this.procChance = effect.procChance;
        this.isOn = effect.isOn;
        this.nextTick = effect.nextTick;
        this.endOfDuration = effect.endOfDuration;
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
