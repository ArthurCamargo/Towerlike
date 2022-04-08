using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Base : LivingEntity
{

    public LifeUI lifeUI;

    void Awake()
    {
        lifeUI = GameObject.Find("LifeBarPanel").GetComponent<LifeUI>();
        lifeUI.maxHealth = startingHealth;
        Debug.Log(lifeUI.maxHealth);
    }

    private void Lose()
    {
        SceneManager.LoadScene("Menu");
    }

    public override void TakeAttack(Attack attack)
    {
        health -= attack.damage;
        lifeUI.UpdateUI(health);

        if (health <= 0 && !dead)
        {
            Die();
            Lose();

        }
    }
}
