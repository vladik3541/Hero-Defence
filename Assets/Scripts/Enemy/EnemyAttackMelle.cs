using System;
using UnityEngine;

public class EnemyAttackMell : MonoBehaviour
{
    [SerializeField, Min(0f)] private float _damage;
    private EnemyControl enemyControl;
    private GameObject _target;
    private void OnEnable()
    {
        enemyControl = GetComponent<EnemyControl>();
        
        enemyControl.OnCurrentTarget += TargetSelection;
    }
    private void OnDisable()
    {
        enemyControl.OnCurrentTarget -= TargetSelection;
    }

    private void TargetSelection(GameObject target)
    {
        _target = target;
    }
    public void AttackUnity()
    {
        if (_target == null) return;
        if (_target.TryGetComponent(out Castle castle))
        {
            castle.TakeDamage(_damage);
        }
        if (_target.TryGetComponent(out UnitHealth unitHealth))
        {
            unitHealth.TakeDamage(_damage);
        }

    }

}
