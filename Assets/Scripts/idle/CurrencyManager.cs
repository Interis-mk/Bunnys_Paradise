using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class CurrencyManager : MonoBehaviour
{
    public float currency = 0;
    public int currencyPerSecond = 0;
    public static CurrencyManager instance;
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private TextMeshProUGUI currencyPerSecondText;
    [SerializeField] private string currencyName = "Carrots: ";
    [SerializeField] private string currencyPerSecondName = "CPS: ";
    

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (currencyText == null || currencyPerSecondText == null)
        {
            //Debug.LogError("one or more SerializeFields are not set in CurrencyManager");
        }
    }

    private void Update()
    {
        currencyText.text = currencyName + currency.ToString();
        currencyPerSecondText.text = currencyPerSecondName + currencyPerSecond.ToString();
    }

    public void TakeCurrency(float amount)
    {
        currency -= amount;
    }

    public void AddCurrency(float amount)
    {
        currency += amount;
    }
}