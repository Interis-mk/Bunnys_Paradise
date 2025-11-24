using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages all interactions in the game. Handles input and coordinates between player and interactables.
/// </summary>
public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get; private set; }
    
    [Header("Input Settings")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private bool allowMouseClick = true;
    
    private List<InteractableObject> nearbyInteractables = new List<InteractableObject>();
    private Camera mainCamera;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        mainCamera = Camera.main;
    }
    
    private void Update()
    {
        HandleInput();
    }
    
    private void HandleInput()
    {
        // todo use correct input system
        // Keyboard interaction
        if (Input.GetKeyDown(interactKey))
        {
            InteractWithNearest();
        }
        
        // Mouse click interaction
        if (allowMouseClick && Input.GetMouseButtonDown(0))
        {
            TryInteractWithClickedObject();
        }
    }
    
    private void InteractWithNearest()
    {
        if (nearbyInteractables.Count > 0)
        {
            // Get the closest interactable
            InteractableObject closest = GetClosestInteractable();
            
            if (closest != null && closest.CanInteract())
            {
                closest.Interact();
            }
        }
    }
    
    private void TryInteractWithClickedObject()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (!hit.collider) return;
        InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            
        // Check if the clicked object is in range and can be interacted with
        if (!interactable || !nearbyInteractables.Contains(interactable) || !interactable.CanInteract()) return;
        interactable.Interact();
    }
    
    private InteractableObject GetClosestInteractable()
    {
        if (nearbyInteractables.Count == 0) return null;
        
        InteractableObject closest = nearbyInteractables[0];
        float closestDistance = Vector2.Distance(closest.transform.position, transform.position);
        
        for (int i = 1; i < nearbyInteractables.Count; i++)
        {
            float distance = Vector2.Distance(nearbyInteractables[i].transform.position, transform.position);
            if (distance < closestDistance)
            {
                closest = nearbyInteractables[i];
                closestDistance = distance;
            }
        }
        
        return closest;
    }
    
    public void RegisterInteractable(InteractableObject interactable)
    {
        if (!nearbyInteractables.Contains(interactable))
        {
            nearbyInteractables.Add(interactable);
        }
    }
    
    public void UnregisterInteractable(InteractableObject interactable)
    {
        if (nearbyInteractables.Contains(interactable))
        {
            nearbyInteractables.Remove(interactable);
        }
    }
}