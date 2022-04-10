using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth;

    float currentHealth;

    public event Action<float> OnHealthChange = delegate { };

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void ModifyHealth(float amount)
    {
        currentHealth += amount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        float currentHealthPercent = currentHealth / maxHealth;
        OnHealthChange(currentHealthPercent);

        if (gameObject.tag == "Player" && amount < 0)
        {
            //FLASH SCREEN RED HERE
        }
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void ResetHealth()
    {
        ModifyHealth(maxHealth - currentHealth);
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetMaxHealth(float health)
    {
        maxHealth = health;
    }
}
