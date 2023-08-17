using System;
using UnityEngine;

public class Bank : MonoBehaviour
{
    private static Bank instance;

    [SerializeField] private int startingBalance = 150;

    [SerializeField] private int currentBalance;
    public int CurrentBalance { get { return instance.currentBalance; } }

    public event Action onBalanceChange;

    private void Awake()
    {
        ManageSingleton();
        currentBalance = startingBalance;
    }

    private void ManageSingleton()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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
