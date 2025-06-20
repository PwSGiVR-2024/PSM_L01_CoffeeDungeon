using UnityEngine;

public class CraftingUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject canCraftSlot;
    [SerializeField] private GameObject missingIngredientsSlot;
    [SerializeField] private Transform slotParent;
    [SerializeField] private RecipeList recipeList;

    private void Start()
    {
        LoadUIRecipeSlots();

        if (CraftingManager.Instance != null && CraftingManager.Instance.inventory != null)
        {
            CraftingManager.Instance.inventory.OnInventoryChanged += RefreshRecipeSlots;
            CraftingManager.Instance.inventory.AmountChanged += RefreshRecipeSlots;
        }
    }

    private void OnDestroy()
    {
        if (CraftingManager.Instance != null && CraftingManager.Instance.inventory != null)
        {
            CraftingManager.Instance.inventory.OnInventoryChanged -= RefreshRecipeSlots;
            CraftingManager.Instance.inventory.AmountChanged -= RefreshRecipeSlots;
        }
    }

    private void RefreshRecipeSlots()
    {
        ClearAllSlots();

        LoadUIRecipeSlots();
    }

    private void ClearAllSlots()
    {
        foreach (Transform child in slotParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void LoadUIRecipeSlots()
    {
        foreach (var recipe in recipeList.recipes)
        {
            bool canCraft = CraftingManager.Instance.CanCraft(recipe);

            GameObject prefabToUse = canCraft ? canCraftSlot : missingIngredientsSlot;

            GameObject slotGO = Instantiate(prefabToUse, slotParent);
            RecipeSlotUI slotUI = slotGO.GetComponent<RecipeSlotUI>();
            slotUI.Setup(recipe);
        }
    }
}