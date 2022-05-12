using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageable {

    public float startingHealth;
    public float health;
    protected bool dead;
    public Image healthBarUI;

    public event System.Action OnDeath;

    protected virtual void Start() {
        health = startingHealth;
    }

    public void TakeHit(Attack attack, RaycastHit hit) {
        TakeAttack(attack);
    }

    public virtual void TakeAttack(Attack attack)
    {
        health -= attack.damage;
        healthBarUI.fillAmount = health/startingHealth;

        if (health <= 0 && !dead)
        {
            //Drop();
            Die();
        }
    }

    public void BaseHit()
    {
        if (!dead)
        {
            Die();
        }
    }

    protected void Die(){
        dead = true;
        if(OnDeath != null) {
            OnDeath();
        }
        GameObject.DestroyImmediate(gameObject);
    }
}
