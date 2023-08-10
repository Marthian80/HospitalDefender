using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BankPresenter : MonoBehaviour
{
    [SerializeField] private Bank bank;

    [SerializeField] TextMeshProUGUI displayBalance;

    private void Start()
    {
        bank.onBalanceChange += UpdateDisplay;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        displayBalance.text = "$ " + bank.CurrentBalance;
    }
}

