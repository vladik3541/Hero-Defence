using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttackMelee : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _damage;
    private UnitControl _UnitControl;
    private GameObject _target;

    private void OnEnable()
    {
        _UnitControl = GetComponent<UnitControl>();

        _UnitControl.OnAttack += TargetSelection;
    }
    private void OnDisable()
    {
        _UnitControl.OnAttack -= TargetSelection;
    }

    private void TargetSelection(GameObject target)
    {
        _target = target;
    }
    public void AttackUnity()
    {
        if(_target == null) return;
        if (_target.TryGetComponent(out Castle castle))
        {
            castle.TakeDamage(_damage);
        }
        if (_target.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(_damage);
        }

    }
}
