using TMPro;
using UnityEngine;

public class BankPresenter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayBalance;

    private void Start()
    {
        Bank.Instance.onBalanceChange += UpdateDisplay;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        displayBalance.text = "$ " + Bank.Instance.CurrentBalance;
    }
}