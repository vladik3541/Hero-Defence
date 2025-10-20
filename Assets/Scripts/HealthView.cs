using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Health health;

    private void OnEnable()
    {
        health.OnHealthChanged += UpdateBar;
    }

    private void OnDisable()
    {
        health.OnHealthChanged -= UpdateBar;
    }

    private void UpdateBar(float normalizedHealth)
    {
        healthBar.fillAmount = normalizedHealth;
    }
}
