using UnityEngine;

/// <summary>
/// Example: Door that can be opened/closed
/// </summary>
public class DoorInteractable : InteractableObject
{
    [Header("Door Settings")]
    [SerializeField] private bool isOpen = false;
    [SerializeField] private bool isLocked = false;
    [SerializeField] private string requiredKey = "";
    
    [Header("Animation")]
    [SerializeField] private Animator doorAnimator;
    
    [Header("Audio")]
    [SerializeField] private AudioClip openSound;
    [SerializeField] private AudioClip closeSound;
    [SerializeField] private AudioClip lockedSound;
    
    private AudioSource audioSource;
    
    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
    }
    
    public override void Interact()
    {
        base.Interact();
        
        if (isLocked)
        {
            // Check if player has the key
            // bool hasKey = InventoryManager.Instance.HasItem(requiredKey);
            bool hasKey = false; // Replace with actual inventory check
            
            if (hasKey)
            {
                isLocked = false;
                Debug.Log("Door unlocked!");
            }
            else
            {
                Debug.Log("Door is locked!");
                PlaySound(lockedSound);
                return;
            }
        }
        
        ToggleDoor();
    }
    
    private void ToggleDoor()
    {
        isOpen = !isOpen;
        
        if (doorAnimator != null)
        {
            doorAnimator.SetBool("IsOpen", isOpen);
        }
        
        PlaySound(isOpen ? openSound : closeSound);
        Debug.Log(isOpen ? "Door opened" : "Door closed");
    }
    
    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    
    public override string GetInteractionPrompt()
    {
        if (isLocked)
        {
            return "Locked";
        }
        return isOpen ? "Close" : "Open";
    }
    
    public override bool CanInteract()
    {
        return base.CanInteract();
    }
}