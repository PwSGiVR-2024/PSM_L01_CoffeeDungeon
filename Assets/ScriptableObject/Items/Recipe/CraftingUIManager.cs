using UnityEngine;

public class CraftingUIManager : MonoBehaviour
{
    [SerializeField] private GameObject canCraftSlot;
    [SerializeField] private GameObject missingIngredientsSlot;
    [SerializeField] private Transform slotParent;
    [SerializeField] private RecipeList recipeList; //from this i need to load from each recipe: icon, name and ingredients to make || to swap in prefab: icon and name || after hover over 
    //recipe slot shows an ingredient list from hover menu???

    private void Start()
    {
        LoadUIRecipeSlots();
    }
    private void LoadUIRecipeSlots()
    {
        foreach (var recipe in recipeList.recipes)
        {
            GameObject slotGO = Instantiate(canCraftSlot, slotParent);
            RecipeSlotUI slotUI = slotGO.GetComponent<RecipeSlotUI>();

            slotUI.Setup(recipe);
        }
    }
}
