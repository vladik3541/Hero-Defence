using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeAttack : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _bulletSpeed;

    [Space(20)]
    [SerializeField] private LayerMask _layerMaskEnemy;
    [SerializeField] private Transform _spawnBullet;
    [Space(20)]
    [SerializeField] private ParticleSystem _shootFX;
    [SerializeField] private GameObject _projectile;

    [SerializeField] private Vector3 _offsetForHitTarget;

    private GameObject _target;

    private UnitControl _UnitControl;

    private void OnEnable()
    {
        _UnitControl = GetComponent<UnitControl>();
        _UnitControl.OnAttack += Target;
    }
    private void OnDisable()
    {
        _UnitControl.OnAttack -= Target;
    }

    private void Target(GameObject target)
    {
        _target = target;
    }

    public void Attack()
    {
        GameObject trail = Instantiate(_projectile, _spawnBullet.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, _target));
 
    }
    
    

    private IEnumerator SpawnTrail(GameObject Trail, GameObject HitPoint)
    {
        if (HitPoint == null) yield break;
        Vector3 target = HitPoint.transform.position;

        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, target);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, target, 1 - (remainingDistance / distance));

            remainingDistance -= _bulletSpeed * Time.deltaTime;

            yield return null;
        }

        Trail.transform.position = target;
        if (HitPoint != null)
        {
            HitPoint.GetComponent<EnemyHealth>().TakeDamage(_damage);
        }
        Destroy(Trail);
    }
}
