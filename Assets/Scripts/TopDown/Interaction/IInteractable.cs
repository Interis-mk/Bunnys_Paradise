using UnityEngine;

/// <summary>
/// Interface that all interactable objects must implement
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Called when the player interacts with this object
    /// </summary>
    void Interact();
    
    /// <summary>
    /// The text to display in the interaction prompt (optional)
    /// </summary>
    string GetInteractionPrompt();
    
    /// <summary>
    /// Whether this object can currently be interacted with
    /// </summary>
    bool CanInteract();
}