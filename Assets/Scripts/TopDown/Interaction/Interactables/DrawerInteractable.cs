using UnityEngine;

/// <summary>
/// Example: Drawer that can be opened to reveal items
/// </summary>
public class DrawerInteractable : InteractableObject
{
    [Header("Drawer Settings")]
    [SerializeField] private bool isOpen = false;
    [SerializeField] private Transform drawerTransform;
    [SerializeField] private Vector3 openPosition;
    [SerializeField] private Vector3 closedPosition;
    [SerializeField] private float openSpeed = 2f;
    
    [Header("Contents")]
    [SerializeField] private GameObject[] itemsInside;
    
    private bool isMoving = false;
    private Vector3 targetPosition;
    
    protected override void Start()
    {
        base.Start();
        
        if (drawerTransform == null)
        {
            drawerTransform = transform;
        }
        
        closedPosition = drawerTransform.localPosition;
        
        // Hide items initially if drawer is closed
        if (!isOpen)
        {
            SetItemsVisibility(false);
        }
    }
    
    protected override void Update()
    {
        base.Update();
        
        if (isMoving)
        {
            drawerTransform.localPosition = Vector3.Lerp(
                drawerTransform.localPosition,
                targetPosition,
                Time.deltaTime * openSpeed
            );
            
            if (Vector3.Distance(drawerTransform.localPosition, targetPosition) < 0.01f)
            {
                drawerTransform.localPosition = targetPosition;
                isMoving = false;
            }
        }
    }
    
    public override void Interact()
    {
        base.Interact();
        
        isOpen = !isOpen;
        targetPosition = isOpen ? openPosition : closedPosition;
        isMoving = true;
        
        SetItemsVisibility(isOpen);
    }
    
    private void SetItemsVisibility(bool visible)
    {
        foreach (GameObject item in itemsInside)
        {
            if (item != null)
            {
                item.SetActive(visible);
            }
        }
    }
    
    public override string GetInteractionPrompt()
    {
        return isOpen ? "Close Drawer" : "Open Drawer";
    }
}