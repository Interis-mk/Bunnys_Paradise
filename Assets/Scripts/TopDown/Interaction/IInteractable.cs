using UnityEngine;
using UnityEngine.Events;

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
    /// Whether this object can currently be interacted with
    /// </summary>
    bool CanInteract();
    
    /// <summary>
    /// The UnityEvent to invoke when interaction occurs 
    /// </summary>
    //UnityEvent OnInteract { get; set;}
}