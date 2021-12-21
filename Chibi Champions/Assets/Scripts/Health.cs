using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        float currentHealthPercent = currentHealth / maxHealth;
        OnHealthChange(currentHealthPercent);
    }
}
