using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashcanUI : MonoBehaviour, IDropHandler
{
    [SerializeField]private Sprite closedTrashcan;
    [SerializeField]private Sprite openTrashcan;
    private Image imageComponent;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        var draggedSlotUI = eventData.pointerDrag?.GetComponent<InventorySlotUI>();
        if (draggedSlotUI == null) return;

        if (draggedSlotUI.assignedSlot != null)
        {
            draggedSlotUI.inventoryReference.RemoveSlot(draggedSlotUI.slotIndex);
            StartCoroutine(WaitForIconChange());
        }
        draggedSlotUI.Setup(null);
    }

    IEnumerator WaitForIconChange()
    {
        imageComponent.sprite = openTrashcan;
        yield return new WaitForSeconds(0.25f);
        imageComponent.sprite = closedTrashcan;

    }
}
