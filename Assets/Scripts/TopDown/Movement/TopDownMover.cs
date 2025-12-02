using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

[RequireComponent(typeof(Collider2D))]
public class TopDownMover : MonoBehaviour
{
    [Header("Movement")] [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    private InputAction moveAction;
    private Vector2 moveValue;

    [SerializeField] SpriteRenderer spriteRenderer;

    private static readonly int IsUpID = Animator.StringToHash("Up");
    private static readonly int IsDownID = Animator.StringToHash("Down");
    private static readonly int IsSideID = Animator.StringToHash("Side");

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        

        moveAction = new InputAction("Move", expectedControlType: "Vector2");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        moveAction.performed += ctx => moveValue = ctx.ReadValue<Vector2>();
        moveAction.canceled += ctx => moveValue = Vector2.zero;
    }

    private void OnEnable()
    {
        moveAction?.Enable();
    }

    private void OnDisable()
    {
        if (moveAction != null)
        {
            moveAction.Disable();
            moveAction.Dispose();
            moveAction = null;
        }
    }

    private void FixedUpdate()
    {
        if (moveValue.sqrMagnitude > 1f)
            moveValue = moveValue.normalized;
        if (moveValue.x > 1 && moveValue.y > 0 || moveValue.y > 1 && moveValue.x > 0)
        {
            Vector2 savedDirection = moveValue;
        }

        if (rb != null)
        {
            rb.velocity = moveValue * speed;
        }
        else
        {
            transform.Translate(moveValue * (speed * Time.fixedDeltaTime));
        }

        if (animator != null)
        {
            UpdateAnimatorDirection(moveValue);
        }
    }

     private void UpdateAnimatorDirection(Vector2 dir)
     {
         if (dir == Vector2.zero)
             return;
    
         animator.SetBool(IsUpID, false);
         animator.SetBool(IsDownID, false);
         animator.SetBool(IsSideID, false);
    
         if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
         {
             animator.SetBool(IsSideID, true);
             if (dir.x < 0)
             {
                 spriteRenderer.flipX = true;
             } else if (dir.x > 0)
             {
                 spriteRenderer.flipX = false;
             }
         }
         else
         {
             if (dir.y > 0f)
                 animator.SetBool(IsUpID, true);
             else
                 animator.SetBool(IsDownID, true);
    
             if (spriteRenderer != null)
                 spriteRenderer.flipX = false;
         }
     }
}