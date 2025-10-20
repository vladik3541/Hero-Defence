using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UnitHealth : MonoBehaviour
{
    public static event Action<GameObject> OnDeadUnit;

    [SerializeField, Min(0)] private float health;
    [SerializeField] private Slider sliderHp;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        sliderHp.maxValue = health;
        sliderHp.value = health;

    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        sliderHp.value = health;
        if (health <= 0f)
        {
            OnDeadUnit?.Invoke(gameObject);
            animator.SetTrigger("Die");
            animator.SetBool("Attack", false);
            Destroy(gameObject, 2.1f);

            DisableComponent();
        }
    }

    private void DisableComponent() 
    {
        GetComponent<SphereCollider>().enabled = false;
        GetComponent<UnitControl>().enabled = false;
        sliderHp.gameObject.SetActive(false);
    }
}
