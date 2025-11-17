using System.Collections;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float shootDelay = 0.5f;

    [Header("Aim")]
    [SerializeField] private Transform turret;
    [SerializeField] private float radiusAttack = 6f;
    [SerializeField] private LayerMask enemyMask;

    [Header("Bullet")]
    [SerializeField] private Transform spawnBullet;
    [SerializeField] private GameObject bulletTrail;

    private float lastShootTime;
    private Quaternion initialTurretRotation;

    void Start()
    {
        initialTurretRotation = turret.rotation;
    }

    void Update()
    {
        GameObject target = FindNearestEnemy();
        if (target == null) return;

        RotateToTarget(target);
        Shoot(target);
    }
    
    private GameObject FindNearestEnemy()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radiusAttack, enemyMask);

        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (var hit in hits)
        {
            float d = Vector3.Distance(transform.position, hit.transform.position);
            if (d < minDist)
            {
                minDist = d;
                nearest = hit.gameObject;
            }
        }

        return nearest;
    }
    
    private void RotateToTarget(GameObject target)
    {
        Vector3 dir = target.transform.position - turret.position;
        dir.y = 0;
        turret.rotation = Quaternion.LookRotation(dir.normalized) * initialTurretRotation;

        // barrel rotation
        Vector3 dir2 = target.transform.position - spawnBullet.position;
        spawnBullet.rotation = Quaternion.LookRotation(dir2.normalized);
    }
    
    private void Shoot(GameObject target)
    {
        if (Time.time < lastShootTime + shootDelay) return;

        GameObject trail = Instantiate(bulletTrail, spawnBullet.position, Quaternion.identity);
        StartCoroutine(BulletTrail(trail, target));

        lastShootTime = Time.time;
    }
    
    private IEnumerator BulletTrail(GameObject trail, GameObject target)
    {
        if (target == null)
        {
            Destroy(trail);
            yield break;
        }

        Vector3 start = trail.transform.position;
        Vector3 end = target.transform.position;

        float distance = Vector3.Distance(start, end);
        float remaining = distance;

        while (remaining > 0 && target != null)
        {
            float t = 1 - (remaining / distance);
            trail.transform.position = Vector3.Lerp(start, end, t);

            remaining -= bulletSpeed * Time.deltaTime;
            yield return null;
        }

        if (target != null)
        {
            EnemyHealth hp = target.GetComponent<EnemyHealth>();
            if (hp != null) hp.TakeDamage(damage);
        }

        Destroy(trail);
    }

    // DEBUG
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radiusAttack);
    }
}
