using System.Collections;
using UnityEngine;

public class RewardDisplayer : MonoBehaviour
{
    [SerializeField] private float animationDuration = 1.2f;

    private BacteriaSpawner bacteriaSpawner;
    private PriceDisplay priceDisplayReward;

    private void Awake()
    {
        bacteriaSpawner = FindObjectOfType<BacteriaSpawner>();
        bacteriaSpawner.enemyKilled += BacteriaSpawner_enemyKilled;
        priceDisplayReward = gameObject.transform.Find("RewardKillBacteria").GetComponent<PriceDisplay>();
    }

    private void BacteriaSpawner_enemyKilled(Vector2 obj)
    {
        transform.position = new Vector3(obj.x, obj.y, 0);
        StartCoroutine(PlayRewardPriceRoutine());
    }

    private IEnumerator PlayRewardPriceRoutine()
    {
        priceDisplayReward.GetComponent<PriceDisplay>().FloatPrice();
        yield return new WaitForSeconds(animationDuration);
        priceDisplayReward.GetComponent<SpriteRenderer>().enabled = false;
    }
}
