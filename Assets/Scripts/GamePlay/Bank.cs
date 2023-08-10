using System;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [SerializeField] private int startingBalance = 150;

    [SerializeField] private int currentBalance;
    public int CurrentBalance { get { return currentBalance; } }

    public event Action onBalanceChange;

    private void Awake()
    {
        currentBalance = startingBalance;        
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        Debug.Log(currentBalance);
        if (onBalanceChange!= null)
        {
            onBalanceChange();
        }
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        Debug.Log(currentBalance);
        if (onBalanceChange != null)
        {
            onBalanceChange();
        }
    }
}
