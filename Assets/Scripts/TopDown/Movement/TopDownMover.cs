using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class TopDownMover : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    private InputAction moveAction;
    private Vector2 moveValue;
    
    private SpriteRenderer spriteRenderer;
    
    private static readonly int IsUpID = Animator.StringToHash("IsUp");
    private static readonly int IsDownID = Animator.StringToHash("IsDown");
    private static readonly int IsLeftID = Animator.StringToHash("IsLeft");

    private void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        moveAction = new InputAction("Move", InputActionType.Value, expectedControlType: "Vector2");
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
        // Only update when moving
        if (dir == Vector2.zero)
            return;

        // Reset
        animator.SetBool(IsUpID, false);
        animator.SetBool(IsDownID, false);
        animator.SetBool(IsLeftID, false);

        // Pick primary axis (simple 4-direction)
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            // Horizontal
            animator.SetBool(IsLeftID, true); 
            if (spriteRenderer != null)
                spriteRenderer.flipX = dir.x > 0f; // flip when moving right
        }
        else
        {
            // Vertical
            if (dir.y > 0f)
                animator.SetBool(IsUpID, true);
            else
                animator.SetBool(IsDownID, true);

            if (spriteRenderer != null)
                spriteRenderer.flipX = false; // ensure consistent vertical frame
        }
    }
}