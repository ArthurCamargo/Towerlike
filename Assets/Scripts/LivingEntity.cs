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
        Color redColor = new Color(0xFF, 0x47, 0x57);
        Color greenColor = new Color(0x7B,0xED,0x9F);
        health -= attack.damage;
        healthBarUI.fillAmount = health/startingHealth;
        healthBarUI.color = Color.Lerp(Color.green, Color.red, healthBarUI.fillAmount);
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
