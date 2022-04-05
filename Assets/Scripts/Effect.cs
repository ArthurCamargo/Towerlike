using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect {
    public bool isOn;
    public float duration;
    public float tickFrequency;
    public float damage;

    private float nextTick;
    private float endOfDuration;

    public Effect(bool isOn, float duration, float tickFrequency, float damage) {
        this.isOn = isOn;
        this.duration = duration;
        this.tickFrequency = tickFrequency;
        this.damage = damage;
    }

    public Effect() {
        this.isOn = false;
        this.duration = 0;
        this.tickFrequency = 0;
        this.damage = 0;
    }

    public void StartEffect(float duration, float tickFrequency, float damage) {
        if(!isOn) {
            this.isOn = true;
            this.duration = duration;
            this.tickFrequency = tickFrequency;
            this.damage = damage;
            nextTick = Time.time + tickFrequency;
            endOfDuration = Time.time + duration;
        }
        else {
            this.duration = duration;
            this.tickFrequency = tickFrequency;
            this.damage = damage;
            endOfDuration = Time.time + duration;
        }
    }

    public bool ExpiredDuration() {
        if(endOfDuration <= Time.time) {
            return true;
        }
        else {
            return false;
        }
    }

    public void UpdateDuration() {
        if (!isOn) {
            return;
        }

        if(ExpiredDuration()) {
            isOn = false;
        }
    }

    public bool Tick() {
        if(isOn && nextTick <= Time.time) {
            nextTick = Time.time + tickFrequency;
            return true;
        }
        else {
            return false;
        }
    }
}
