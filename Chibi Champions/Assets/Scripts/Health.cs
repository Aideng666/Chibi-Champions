using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth;

    float currentHealth;

    [SerializeField] public Image hurtImage;
    Color splatterAlpha;

    public event Action<float> OnHealthChange = delegate { };

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        if (hurtImage != null)
        {
            splatterAlpha = hurtImage.color;
        }         
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
            TakeDamage();
        }
    }

    void UpdateHealth()
    {
        if (hurtImage != null)
        {
            splatterAlpha = hurtImage.color;

            if (currentHealth <= 60)
            {
                splatterAlpha.a = 1 - (currentHealth / maxHealth);
                hurtImage.color = splatterAlpha;
            }      

            if (currentHealth <= 0)
            {
                splatterAlpha.a = 0;
                hurtImage.color = splatterAlpha;
            }
        }       
    }

    public void TakeDamage()
    {
        if (currentHealth >= 0)
        {
            UpdateHealth();
        }
    }

    private void Update()
    {
        if (currentHealth <= maxHealth - 0.01)
        {
            UpdateHealth();
        }
        else 
        {
            currentHealth = maxHealth;
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
