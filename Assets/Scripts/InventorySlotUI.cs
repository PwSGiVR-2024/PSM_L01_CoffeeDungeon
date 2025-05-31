using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image icon;
    public TMP_Text amountText;
    public InventorySlot assignedSlot;

    public int slotIndex;
    [SerializeField]public Inventory inventoryReference; 

    private Transform ogParent;
    private CanvasGroup canvasGroup;

    private GameObject inventoryUI;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        inventoryUI = GameObject.Find("InventoryUI");
    }

    public void Setup(InventorySlot slot)
    {
        assignedSlot = slot;

        if (slot == null || slot.item == null)
        {
            icon.sprite = null;
            icon.enabled = false;
            amountText.text = "";
            return;
        }

        icon.sprite = slot.item.iconPrefab;
        icon.enabled = true;
        amountText.text = slot.amount.ToString();
    }

    private bool IsInventoryOpen()
    {
        return inventoryUI != null && inventoryUI.activeInHierarchy;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!IsInventoryOpen()) return;

        ogParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!IsInventoryOpen()) return;

        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!IsInventoryOpen()) return;

        transform.SetParent(ogParent);
        transform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!IsInventoryOpen()) return;

        InventorySlotUI draggedSlotUI = eventData.pointerDrag?.GetComponent<InventorySlotUI>();
        if (draggedSlotUI == null || draggedSlotUI == this) return;

        if (draggedSlotUI.inventoryReference != null && inventoryReference != null)
        {
            InventorySlot tempSlot = inventoryReference.slots[slotIndex];
            inventoryReference.slots[slotIndex] = draggedSlotUI.inventoryReference.slots[draggedSlotUI.slotIndex];
            draggedSlotUI.inventoryReference.slots[draggedSlotUI.slotIndex] = tempSlot;

            Setup(inventoryReference.slots[slotIndex]);
            draggedSlotUI.Setup(draggedSlotUI.inventoryReference.slots[draggedSlotUI.slotIndex]);
        }
        else
        {
            var temp = assignedSlot;
            assignedSlot = draggedSlotUI.assignedSlot;
            draggedSlotUI.assignedSlot = temp;

            Setup(assignedSlot);
            draggedSlotUI.Setup(draggedSlotUI.assignedSlot);
        }
    }
}
