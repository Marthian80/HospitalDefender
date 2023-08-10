using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoapBullet : MonoBehaviour
{
    [SerializeField] private float projectileRange = 15f;
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private GameObject particleOnHitVFX;
    
    private Vector3 startPosition;

    public Transform Target;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        //Indestructable indestructable = collision.gameObject.GetComponent<Indestructable>();

        if (!collision.isTrigger && (enemyHealth)) //|| indestructable)
        {
            if (enemyHealth)
            {
                ProjectileHitDetected();
                enemyHealth.TakeDamage(1);
            }
        }
    }

    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > this.projectileRange)
        {
            Destroy(gameObject);
        }
    }

    private void MoveProjectile()
    {
        transform.position = Vector2.MoveTowards(transform.position, Target.position, moveSpeed * Time.deltaTime);
        //transform.Translate(Target.position * moveSpeed * Time.deltaTime);
    }

    private void ProjectileHitDetected()
    {
        //Instantiate(particleOnHitVFX, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
