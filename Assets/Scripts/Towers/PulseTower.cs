using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PulseTower : Tower {
    public Transform attackPrefab;

    protected override void Awake() {
        base.Awake();
    }

    protected override void Start() {
        base.Start();
    }

    protected override void Update() {
        base.Update();
    }

    public override void Attack() {
        Pulse();
    }

    public void Pulse() {
        //Create sphere as pulse animation
        var animation = new GameObject().AddComponent<Animation>();
        animation.SetAnimation(attackPrefab, attackPlaceHolder.transform.position, (1 / attributes.attackSpeed)*0.40f, attributes.range);
        FindObjectOfType<AudioManager>().Play("Pulse");

        //Do damage on enemies
        Explode(attackPlaceHolder.transform.position);

    }

    public override void BeforeDestroy() {
    }
}