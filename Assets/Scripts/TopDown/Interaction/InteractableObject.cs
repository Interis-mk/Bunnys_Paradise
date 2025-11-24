using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    [Header("Interaction Settings")]
    public virtual string interactionPrompt = "Interact";
    public virtual bool isInteractable = true;
    public virtual float interactionRadius = 2f;

    [Header("Visual Feedback")]
    public virtual Sprite interactionSprite;
    public virtual Vector3 spriteOffset = new Vector3(0, 1, 0);
    public virtual float spriteScale = 0.5f;
    public virtual int sortingOrder = 100;
    public virtual string sortingLayerName = "UI";

    protected bool playerInRange = false;
    protected Transform playerTransform;
    protected GameObject interactionPromptObject;
    protected SpriteRenderer promptSpriteRenderer;
    public UnityEvent OnInteract { get; set; }

    protected virtual void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
        OnInteract.AddListener(OnEventTriggerd);
        CreateInteractionPrompt();
    }

    protected virtual void CreateInteractionPrompt()
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
            promptSpriteRenderer.sortingLayerName = sortingLayerName;

        interactionPromptObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (playerInRange && interactionPromptObject != null && interactionPromptObject.activeSelf)
        {
            float bobAmount = Mathf.Sin(Time.time * 3f) * 0.1f;
            interactionPromptObject.transform.localPosition = spriteOffset + new Vector3(0, bobAmount, 0);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && CanInteract())
        {
            playerInRange = true;
            playerTransform = other.transform;
            ShowInteractionUI();
            InteractionManager.Instance?.RegisterInteractable(this);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
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

    public virtual void SetInteractable(bool value)
    {
        isInteractable = value;

        if (!value && playerInRange)
            HideInteractionUI();
        else if (value && playerInRange)
            ShowInteractionUI();
    }

    protected virtual void ShowInteractionUI()
    {
        if (interactionPromptObject != null)
            interactionPromptObject.SetActive(true);
    }

    protected virtual void HideInteractionUI()
    {
        if (interactionPromptObject != null)
            interactionPromptObject.SetActive(false);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    protected virtual void OnEventTriggerd()
    {
    }
}
