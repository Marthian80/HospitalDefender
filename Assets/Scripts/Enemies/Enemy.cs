using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = 25;
    [SerializeField] int goldPenalty = 25;

    public void RewardGold()
    {        
        Bank.Instance.Deposit(goldReward);
    }

    public void StealGold()
    {
        if (Bank.Instance.CurrentBalance <= 0) { return; }
        Bank.Instance.Withdraw(goldPenalty);
    }
}