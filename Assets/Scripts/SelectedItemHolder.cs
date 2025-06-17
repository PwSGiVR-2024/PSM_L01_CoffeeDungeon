using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class SelectedItemHolder : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private RecipeList recipeList;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TMP_Text itemNameText;

    [Header("Visual Feedback")]
    [SerializeField] private Color validDropColor = Color.green;
    [SerializeField] private Color invalidDropColor = Color.red;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.yellow;


    private ItemData currentHeldItem;
    private GameObject currentSpawnedModel;
    private Image backgroundImage;
    private bool isDragOver = false;

    private void Awake()
    {
        if (!TryGetComponent<Image>(out backgroundImage))
        {
            Debug.LogError("SelectedItemHolder: No Image component found! This is required for drop detection.");
        }

        // Ensure raycast target is enabled
        if (backgroundImage != null && !backgroundImage.raycastTarget)
        {
            Debug.LogWarning("SelectedItemHolder: Image raycastTarget is disabled. Enabling it for drop detection.");
            backgroundImage.raycastTarget = true;
        }
    }

    private void Start()
    {
        ClearSelectedItem();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (eventData.pointerDrag != null)
        {
            isDragOver = true;
            InventorySlotUI draggedSlot = eventData.pointerDrag.GetComponent<InventorySlotUI>();
            if (draggedSlot != null && draggedSlot.assignedSlot != null && draggedSlot.assignedSlot.item != null)
            {
                bool isValidDrop = IsItemCraftable(draggedSlot.assignedSlot.item);
                ShowDropFeedback(isValidDrop ? validDropColor : hoverColor);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragOver)
        {
            isDragOver = false;
            ResetBackgroundColor();
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        isDragOver = false;

        if (eventData.pointerDrag == null)
        {
            ShowDropFeedback(false);
            ResetBackgroundColorDelayed();
            return;
        }

        InventorySlotUI draggedSlot = eventData.pointerDrag.GetComponent<InventorySlotUI>();

        if (draggedSlot == null)
        {
            ShowDropFeedback(false);
            ResetBackgroundColorDelayed();
            return;
        }

        if (draggedSlot.assignedSlot == null)
        {
            ShowDropFeedback(false);
            ResetBackgroundColorDelayed();
            return;
        }

        if (draggedSlot.assignedSlot.item == null)
        {
            ShowDropFeedback(false);
            ResetBackgroundColorDelayed();
            return;
        }

        ItemData draggedItem = draggedSlot.assignedSlot.item;

        if (IsItemCraftable(draggedItem))
        {

            SetSelectedItem(draggedItem);
            ShowDropFeedback(true);
        }
        else
        {
            ShowDropFeedback(false);
        }

        ResetBackgroundColorDelayed();
    }

    private bool IsItemCraftable(ItemData item)
    {
        if (recipeList == null || recipeList.recipes == null)
        {
            return false;
        }

        foreach (CraftingRecipe recipe in recipeList.recipes)
        {
            if (recipe.result == item)
                return true;
        }
        return false;
    }

    private void SetSelectedItem(ItemData item)
    {
        
        ClearSelectedItemInternal();

        currentHeldItem = item;

        if (itemIcon != null)
        {
            itemIcon.sprite = item.iconPrefab;
            itemIcon.enabled = true;
        }

        if (itemNameText != null)
        {
            itemNameText.text = item.name;
        }

        SpawnModel(item);
    }

    private void SpawnModel(ItemData item)
    {
        if (item.inGameObject != null && spawnPoint != null)
        { 
            currentSpawnedModel = Instantiate(item.inGameObject, spawnPoint);
            currentSpawnedModel.transform.localPosition = Vector3.zero;
            currentSpawnedModel.transform.localRotation = Quaternion.identity;

        }
    }

    private void ClearSelectedItemInternal()
    { 
        currentHeldItem = null;

        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.enabled = false;
        }

        if (itemNameText != null)
        {
            itemNameText.text = "Selected Item To Hold";
        }

        if (currentSpawnedModel != null)
        {
            DestroyImmediate(currentSpawnedModel);
            currentSpawnedModel = null;
        }
    }

    public void ClearSelectedItem()
    {
        ClearSelectedItemInternal();
        ResetBackgroundColor();
    }

    public void OnClearButtonPressed()
    {
        ClearSelectedItem();
    }

    private void ShowDropFeedback(bool isValid)
    {
        ShowDropFeedback(isValid ? validDropColor : invalidDropColor);
    }

    private void ShowDropFeedback(Color color)
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = color;
        }
    }

    private void ResetBackgroundColorDelayed()
    {
        CancelInvoke(nameof(ResetBackgroundColor));
        Invoke(nameof(ResetBackgroundColor), 0.5f);
    }

    private void ResetBackgroundColor()
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = normalColor;
        }
    }

    public ItemData GetCurrentHeldItem()
    {
        return currentHeldItem;
    }

    public bool HasItemSelected()
    {
        return currentHeldItem != null;
    }
}