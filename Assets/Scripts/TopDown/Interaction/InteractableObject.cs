using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("Interaction Settings")]
    [SerializeField] private string interactionPrompt = "Interact";
    [SerializeField] private bool isInteractable = true;
    [SerializeField] private float interactionRadius = 2f;
    
    [Header("Visual Feedback")]
    [SerializeField] private Sprite interactionSprite;
    [SerializeField] private Vector3 spriteOffset = new Vector3(0, 1, 0);
    [SerializeField] private float spriteScale = 0.5f;
    [SerializeField] private int sortingOrder = 100;
    [SerializeField] private string sortingLayerName = "UI"; // Optional
    
    private bool playerInRange = false;
    private Transform playerTransform;
    private GameObject interactionPromptObject;
    private SpriteRenderer promptSpriteRenderer;
    
    protected virtual void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
        
        CreateInteractionPrompt();
    }
    
    private void CreateInteractionPrompt()
    {
        if (interactionSprite == null) return;
        
        interactionPromptObject = new GameObject("InteractionE");
        interactionPromptObject.transform.SetParent(transform);
        interactionPromptObject.transform.localPosition = spriteOffset;
        interactionPromptObject.transform.localScale = Vector3.one * spriteScale;
        
        promptSpriteRenderer = interactionPromptObject.AddComponent<SpriteRenderer>();
        promptSpriteRenderer.sprite = interactionSprite;
        promptSpriteRenderer.sortingOrder = sortingOrder;
        
        if (!string.IsNullOrEmpty(sortingLayerName))
        {
            promptSpriteRenderer.sortingLayerName = sortingLayerName;
        }
        
        interactionPromptObject.SetActive(false);
    }
    
    protected virtual void Update()
    {
        // Optional
        if (playerInRange && interactionPromptObject != null && interactionPromptObject.activeSelf)
        {
            float bobAmount = Mathf.Sin(Time.time * 3f) * 0.1f;
            interactionPromptObject.transform.localPosition = spriteOffset + new Vector3(0, bobAmount, 0);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && CanInteract())
        {
            playerInRange = true;
            playerTransform = other.transform;
            ShowInteractionUI();
            
            InteractionManager.Instance?.RegisterInteractable(this);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerTransform = null;
            HideInteractionUI();
            
            InteractionManager.Instance?.UnregisterInteractable(this);
        }
    }
    
    public virtual void Interact()
    {
        Debug.Log($"Interacted with {gameObject.name}");
    }
    
    public virtual string GetInteractionPrompt()
    {
        return interactionPrompt;
    }
    
    public virtual bool CanInteract()
    {
        return isInteractable;
    }
    
    public void SetInteractable(bool value)
    {
        isInteractable = value;
        
        if (!value && playerInRange)
        {
            HideInteractionUI();
        }
        else if (value && playerInRange)
        {
            ShowInteractionUI();
        }
    }
    
    protected virtual void ShowInteractionUI()
    {
        if (interactionPromptObject != null)
        {
            interactionPromptObject.SetActive(true);
        }
    }
    
    protected virtual void HideInteractionUI()
    {
        if (interactionPromptObject != null)
        {
            interactionPromptObject.SetActive(false);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        // in editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}