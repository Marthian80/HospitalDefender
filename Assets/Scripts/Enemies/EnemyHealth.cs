using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHitPoints = 5;
    [SerializeField] private GameObject deathVFX;

    private Flash flash;
    private Enemy enemy;
    private int currentHitPoints = 0;

    private void Awake()
    {     
        enemy= GetComponent<Enemy>();
        flash = GetComponent<Flash>();
    }

    private void OnEnable()
    {
        currentHitPoints = maxHitPoints;
    }

    public void TakeDamage(int damage)
    {
        currentHitPoints -= damage;
        StartCoroutine(flash.FlashRoutine());
        StartCoroutine(CheckDetectDeathRoutine());
    }

    private IEnumerator CheckDetectDeathRoutine()
    {
        yield return new WaitForSeconds(flash.GetRestoreDefaultMatTime());
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (currentHitPoints <= 0)
        {
            Instantiate(deathVFX, transform.position, Quaternion.identity);
            enemy.RewardGold();
            gameObject.SetActive(false);
        }
    }
}
