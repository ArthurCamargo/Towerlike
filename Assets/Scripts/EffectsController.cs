using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsController {
    public Effect burn;
    public Effect bleed;
    public Effect slow;
    public Effect poison;
    public Effect stun;
    public Effect fear;
    public Effect curse;

    public EffectsController() {
        this.burn = new Effect();
        this.bleed = new Effect();
        this.slow = new Effect();
        this.poison = new Effect();
        this.stun = new Effect();
        this.fear = new Effect();
        this.curse = new Effect();
    }

    public void UpdateDurations() {
        burn.UpdateDuration();
        bleed.UpdateDuration();
        slow.UpdateDuration();
        poison.UpdateDuration();
        //stun.UpdateDuration();
        fear.UpdateDuration();
        curse.UpdateDuration();
    }
}
