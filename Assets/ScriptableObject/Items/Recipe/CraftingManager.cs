using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public Inventory inventory;

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
