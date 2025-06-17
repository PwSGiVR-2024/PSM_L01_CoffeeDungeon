using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private GameObject ingredientList;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject ingredientRowPrefab;
    [SerializeField] private Button craftButton;

    private CraftingRecipe recipe;

    public void Setup(CraftingRecipe recipe)
    {
        this.recipe = recipe;

        iconImage.sprite = recipe.result.iconPrefab;
        nameText.text = recipe.result.itemName;

        if (craftButton != null)
        {
            craftButton.onClick.RemoveAllListeners();
            craftButton.onClick.AddListener(OnCraftButtonClicked);

            craftButton.interactable = CraftingManager.Instance.CanCraft(recipe);
        }
    }

    private void OnCraftButtonClicked()
    {
        if (CraftingManager.Instance != null && recipe != null)
        {
            CraftingManager.Instance.Craft(recipe);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ingredientList.SetActive(true);
        LoadIngredientsList();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ingredientList.SetActive(false);
        ClearIngredientsList();
    }

    private void LoadIngredientsList()
    {
        foreach (var ingredient in recipe.ingredients)
        {
            GameObject row = Instantiate(ingredientRowPrefab, content);

            if (row.TryGetComponent<TMP_Text>(out var rowText))
            {
                rowText.text = $"{ingredient.item.itemName} x{ingredient.quantity}";
            }
            else
            {
                Debug.LogWarning("TMP_Text component not found in ingredientRowPrefab.");
            }
        }
    }

    private void ClearIngredientsList()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}
