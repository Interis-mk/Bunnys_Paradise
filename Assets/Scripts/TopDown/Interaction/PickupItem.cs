using UnityEngine;


public class PickupItem : InteractableObject
{
    [Header("Pickup Settings")]
    [SerializeField] private string itemName = "Item";
    [SerializeField] private Sprite itemIcon;
    [SerializeField] private bool destroyOnPickup = true;
    
    public override void Interact()
    {
        base.Interact();
        
        // Add to player inventory (you'll need to implement your inventory system)
        //Debug.Log($"Picked up {itemName}");
        // InventoryManager.Instance.AddItem(itemName, itemIcon);
        
        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
    }
}