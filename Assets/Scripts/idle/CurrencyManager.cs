using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    private float currency = 0;
    
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    public void AddCurrency(float amount)
    {
        currency += amount;
    }
}