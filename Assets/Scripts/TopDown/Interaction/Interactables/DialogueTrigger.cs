using UnityEngine;

/// <summary>
/// Example: Trigger dialogue with an NPC or object
/// </summary>
public class DialogueTrigger : InteractableObject
{
    [Header("Dialogue Settings")]
    [SerializeField] private string npcName = "NPC";
    [TextArea(3, 10)]
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private bool canOnlyTriggerOnce = false;
    
    private bool hasTriggered = false;
    
    public override void Interact()
    {
        base.Interact();
        
        if (canOnlyTriggerOnce && hasTriggered)
        {
            return;
        }
        
        // Trigger dialogue system (you'll need to implement this)
        // DialogueManager.Instance.StartDialogue(npcName, dialogueLines);
        
        hasTriggered = true;
        
        if (canOnlyTriggerOnce)
        {
            SetInteractable(false);
        }
    }
    
    public override bool CanInteract()
    {
        if (canOnlyTriggerOnce && hasTriggered)
        {
            return false;
        }
        return base.CanInteract();
    }
}