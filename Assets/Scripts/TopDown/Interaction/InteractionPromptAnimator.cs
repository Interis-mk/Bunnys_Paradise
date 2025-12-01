using UnityEngine;

/// <summary>
/// Optional: Add this to the interaction prompt GameObject for animations
/// Attach this to the prompt sprite GameObject
/// </summary>
public class InteractionPromptAnimator : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private bool enableBobbing = true;
    [SerializeField] private float bobSpeed = 3f;
    [SerializeField] private float bobHeight = 0.1f;
    
    [SerializeField] private bool enablePulse = false;
    [SerializeField] private float pulseSpeed = 2f;
    [SerializeField] private float pulseAmount = 0.2f;
    
    [SerializeField] private bool enableRotation = false;
    [SerializeField] private float rotationSpeed = 90f;
    
    private Vector3 startPosition;
    private Vector3 startScale;
    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        startPosition = transform.localPosition;
        startScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        if (enableBobbing)
        {
            ApplyBobbing();
        }
        
        if (enablePulse)
        {
            ApplyPulse();
        }
        
        if (enableRotation)
        {
            ApplyRotation();
        }
    }
    
    private void ApplyBobbing()
    {
        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.localPosition = startPosition + new Vector3(0, bobOffset, 0);
    }
    
    private void ApplyPulse()
    {
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = startScale * pulse;
    }
    
    private void ApplyRotation()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
    
    private void OnEnable()
    {
        // Reset position when enabled
        if (startPosition != Vector3.zero)
        {
            transform.localPosition = startPosition;
        }
    }
}