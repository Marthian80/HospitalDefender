using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHitPoints = 5;    

    private int currentHitPoints = 0;        

    private void OnEnable()
    {
        currentHitPoints = maxHitPoints;
    }

    public void TakeDamage(int damage)
    {
        currentHitPoints -= damage;
        DetectDeath();
    }

    //private void OnParticleCollision(GameObject other)
    //{
    //    EnemyHit();

    //    if (currentHitPoints <= 0)
    //    {
    //        DestroyEnemy();
    //    }
    //}

    private void DetectDeath()
    {
        if (currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
