using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(Animator))]

public class ClickerBase : MonoBehaviour
{
    private CircleCollider2D collider;
    private Animator animator;
    public UnityEvent onClick;

    private void Start()
    {
        onClick.AddListener(OnEventTriggered);
    }
    
    void OnEventTriggered()
    {
        return;
    }
}
