using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance { get; private set; }
    public Inventory inventory;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public bool CanCraft(CraftingRecipe recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            if (!inventory.HasItem(ingredient.item, ingredient.quantity))
                return false;
        }
        return true;
    }

    public void Craft(CraftingRecipe recipe)
    {
        if (!CanCraft(recipe))
        {
            Debug.Log("Cannot craft: missing ingredients.");
            return;
        }

        foreach (var ingredient in recipe.ingredients)
        {
            inventory.RemoveItem(ingredient.item, ingredient.quantity);
        }

        inventory.AddItem(recipe.result, recipe.resultQuantity);
    }
}
