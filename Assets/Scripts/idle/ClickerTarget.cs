using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Animator))]

public class ClickerBase : MonoBehaviour
{
    private CircleCollider2D collider;
    private Animator animator;
    public UnityEvent onClick;

    private void Start()
    {
        onClick = new UnityEvent();
        onClick.AddListener(OnEventTriggered);
    }
    
    void OnEventTriggered()
    {
        Debug.Log("Event Triggered");
    }
}
