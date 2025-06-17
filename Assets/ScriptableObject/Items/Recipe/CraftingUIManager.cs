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
