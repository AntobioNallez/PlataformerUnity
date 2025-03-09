using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    public HealthUI healthUI;
    private SpriteRenderer spriteRenderer;
    public static event Action OnPlayerDie;
    // Start is called before the first frame update
    void Start()
    {
        ResetHealth();

        spriteRenderer = GetComponent<SpriteRenderer>();
        GameController.OnReset += ResetHealth;
        HealthItem.OnHealthCollect += Heal;
        JugadorScript.DeathBarrier += TakeDamage;
    }

    void ResetHealth()
    {
        currentHealth = maxHealth;
        healthUI.SetMaxHearts(maxHealth);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemigoController enemigo = collision.GetComponent<EnemigoController>();
        if (enemigo)
        {
            TakeDamage(enemigo.damage);
        }
        TrapScript trap = collision.GetComponent<TrapScript>();
        if(trap && trap.damage > 0) {
            TakeDamage(trap.damage);
        }
    }

    void Heal(int ammount)
    {
        currentHealth += ammount;
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        healthUI.UpdateHearts(currentHealth);
    }

    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthUI.UpdateHearts(currentHealth);

        //Flash Red
        StartCoroutine(FlashRed());

        if (currentHealth <= 0)
        {
            //se acabo
            OnPlayerDie.Invoke();
        }
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }
}
