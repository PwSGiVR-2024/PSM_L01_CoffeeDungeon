using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashcanUI : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        var draggedSlotUI = eventData?.pointerDrag.GetComponent<InventorySlotUI>();
        if (draggedSlotUI == null) return;

        if (draggedSlotUI.assignedSlot != null)
        {
            draggedSlotUI.inventory.RemoveSlot(draggedSlotUI.slotIndex);
        }
        draggedSlotUI.Setup(null);
    }
}
