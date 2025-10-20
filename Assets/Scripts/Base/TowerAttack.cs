using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shootDelay;
    [Space(20)]
    [SerializeField] private Transform _turret;
    [SerializeField] private float _radiusAttack;
    [Space(20)]
    [SerializeField] private LayerMask _layerMaskEnemy;
    [SerializeField] private Transform _spawnBullet;
    [Space(20)]
    [SerializeField] private ParticleSystem _shootFX;
    [SerializeField] private GameObject _bulletTrail;

    [SerializeField] private Vector3 _offsetForHitTarget;

    private List<GameObject> _targets;


    private float _lastShootTime;
    private Quaternion initialRotation;
    

    private void Start()
    {
        _targets = new List<GameObject>();
        initialRotation = _turret.transform.rotation;
    }
    private void Update()
    {
        if(!_targets.Any()) return;

        if (_targets[0] == null) //���� 1 ������� null �������� �� ������
        {
            _targets.RemoveAt(0);
            Debug.Log("Remove");
            return;
        }

        Attack();
        FollowToTarget();
    }

    private void Attack()
    {

        if (_lastShootTime + _shootDelay < Time.time)
        {

            GameObject trail = Instantiate(_bulletTrail, _spawnBullet.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, _targets[0]));
            

            _lastShootTime = Time.time;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out EnemyHealth enemyHealth))
        {
            _targets.Add(other.gameObject);
        }
    }
    private void RemoveTarget(GameObject enemy)
    {
        _targets.Remove(enemy);
    }

    private void FollowToTarget()
    {

        Vector3 directionToTarget = _targets[0].transform.position - _turret.transform.position;
        directionToTarget.y = 0;

        _turret.transform.rotation = Quaternion.LookRotation(directionToTarget.normalized) * initialRotation;

        FollowRaycastToTarget();
 
    }

    private void FollowRaycastToTarget()
    {

        Vector3 directionToTarget = _targets[0].transform.position - _spawnBullet.position;
        directionToTarget.x = 0;

        _spawnBullet.transform.rotation = Quaternion.LookRotation(directionToTarget.normalized) * initialRotation;
    }

    private IEnumerator SpawnTrail(GameObject Trail, GameObject HitPoint)
    {
        Vector3 target = HitPoint.transform.position;
        if (target == null) yield break;

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
        if(HitPoint != null)
        {
            HitPoint.GetComponent<EnemyHealth>().TakeDamage(_damage);
        }
        Destroy(Trail.gameObject);
    }

}
