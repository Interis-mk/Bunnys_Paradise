using UnityEngine;
[RequireComponent(typeof(Animator))]
public class Carrot : MonoBehaviour,  IClickable
{
    public float multiplier = 1;
    public float baseAmount = 1;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void IsClicked()
    {
        CurrencyManager.instance.AddCurrency(baseAmount * multiplier);
    }
    
}
