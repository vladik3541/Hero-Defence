using System;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public event Action<float> OnHealthChanged;
    public event Action OnDead;
    [SerializeField] protected float maxHealth;
    protected float currentHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth/maxHealth);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        OnHealthChanged?.Invoke(currentHealth/maxHealth);
        if (currentHealth <= 0f)
        {
            OnDead?.Invoke();
            Death();
        }
    }
    protected abstract void Death();
}
