using UnityEngine;

public class AutoCurrencyGenerator : MonoBehaviour
{
    [SerializeField] private float CurrencyPerSecond = 1;
    private float timer = 0;
    private CurrencyManager currencyManager;
    
    private void Start()
    {
        currencyManager = FindObjectOfType<CurrencyManager>();
    }
    
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1)
        {
            timer = 0;
            currencyManager.AddCurrency(CurrencyPerSecond);
        }
    }
}