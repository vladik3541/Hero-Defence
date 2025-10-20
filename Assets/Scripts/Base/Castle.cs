using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _health = 3000;
    [SerializeField] private Slider slider;

    private void Start()
    {
        slider.maxValue = _health;
        slider.value = _health;
    }
    public void TakeDamage(float damage)
    {
        _health -= damage;
        slider.value = _health;
    }

}
