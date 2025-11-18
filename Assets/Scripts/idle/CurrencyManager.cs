using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField]private float currency = 0;
    public static CurrencyManager instance;
    [SerializeField]private TextMeshProUGUI currencyText;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        currencyText.text = "Currency: " + currency.ToString();
    }

    public void AddCurrency(float amount)
    {
        currency += amount;
    }
}