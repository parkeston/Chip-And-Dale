using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private UnityEvent onTakeDamage;
    [SerializeField] private UnityEvent onDead;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            onDead?.Invoke();
        }
        else
        {
            onTakeDamage?.Invoke();
        }
    }
}
