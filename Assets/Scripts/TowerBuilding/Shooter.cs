using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private float range = 5f;
    [SerializeField] private GameObject bulletPreFab;    
    [SerializeField] private float attackCooldown = 2;

    private Transform target;
    private float targetAngle;
    private bool canAttack = true;

    private void Update()
    {
        FindClosestTarget();
        Attack();
    }

    private void FindClosestTarget()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (var enemy in enemies)
        {
            float targetDistance = Vector2.Distance(transform.position, enemy.transform.position);

            if (targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }
        target = closestTarget;
    }

    private void Attack()
    {
        if (target == null) { return; }
        float targetDistance = Vector2.Distance(transform.position, target.position);
        Vector2 targetDirection = target.position - transform.position;
        targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

        if (targetDistance <= range && canAttack)
        {
            canAttack = false;
            var newSoapBullet = Instantiate(bulletPreFab, gameObject.transform.position, quaternion.Euler(0, 0, -targetAngle));
            newSoapBullet.GetComponent<SoapBullet>().Target = target;
            AudioPlayer.Instance.PlayShootSoapClip();
            StartCoroutine(AttackCoolDownRoutine());                       
        }
    }       

    private IEnumerator AttackCoolDownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}
