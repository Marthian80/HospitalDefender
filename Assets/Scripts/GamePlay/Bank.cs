using System;
using UnityEngine;

public class Bank : Singleton<Bank>
{
    [SerializeField] private int startingBalance = 150;

    [SerializeField] private int currentBalance;
    public int CurrentBalance { get { return currentBalance; } }

    public event Action onBalanceChange;

    protected override void Awake()
    {
        base.Awake();        
        currentBalance = startingBalance;
    }

    public void ResetToStartingBalance()
    {
        currentBalance = startingBalance;
        if (onBalanceChange != null)
        {
            onBalanceChange();
        }
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);        
        if (onBalanceChange!= null)
        {
            onBalanceChange();
        }
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);        
        if (onBalanceChange != null)
        {
            onBalanceChange();
        }
    }
}