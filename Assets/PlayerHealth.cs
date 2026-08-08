using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;

    public TMP_Text healthText;
    public Animator healthTextAnim;

    public HealthBar healthBar; 

    private void Start()
    {
        currentHealth = maxHealth;

        UpdateHealthUI();

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthTextAnim != null)
            healthTextAnim.Play("TextUpdate");

        UpdateHealthUI();

       
        if (healthBar != null)
        {
            if (amount < 0)
                healthBar.TakeDamage();
            else if (amount > 0)
                healthBar.OnHealthIncreased();
        }

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false); 
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
            healthText.text = "HP: " + currentHealth + "/" + maxHealth;
    }
}
