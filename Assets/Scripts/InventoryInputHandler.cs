using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class InventoryInputHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private InputActionReference PickUpItem;

    private bool itemInPlayerRange = false;


    private GameObject currentItemObject;
    private ItemData currentItemData;

    public event Action InItemTrigger;
    public event Action LeaveItemTrigger;

    private void OnEnable()
    {
        PickUpItem.action.performed += OnAddItem;
        PickUpItem.action.Enable();
    }

    private void OnDisable()
    {
        PickUpItem.action.performed -= OnAddItem;
        PickUpItem.action.Disable();
    }

    private void OnAddItem(InputAction.CallbackContext context)
    {

        if (itemInPlayerRange && currentItemData != null)
        {
            inventory.AddItem(currentItemData, 1);
            Destroy(currentItemObject);
            currentItemObject = null;
            currentItemData = null;
            itemInPlayerRange = false;
            LeaveItemTrigger?.Invoke();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            AttachData itemComponent = other.GetComponent<AttachData>();
            if (itemComponent != null)
            {
                itemInPlayerRange = true;
                currentItemObject = other.gameObject;
                currentItemData = itemComponent.attachedData;
                InItemTrigger?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentItemObject)
        {
            itemInPlayerRange = false;
            currentItemObject = null;
            currentItemData = null;
            LeaveItemTrigger?.Invoke();
        }
    }

    //private void OnApplicationQuit()
    //{
    //    inventory.slots.Clear();
    //}
}

