using UnityEngine;

public class CraftingTester : MonoBehaviour
{
    public CraftingManager craftingManager;
    public CraftingRecipe recipeToTest;

    void Start()
    {
        Debug.Log("Testing crafting at Start...");

        if (craftingManager.CanCraft(recipeToTest))
        {
            Debug.Log("Can craft the item. Crafting now...");
            craftingManager.Craft(recipeToTest);
        }
        else
        {
            Debug.Log("Cannot craft the item. Missing ingredients.");
        }
    }

    // Optional: Craft on key press
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Craft key pressed.");

            if (craftingManager.CanCraft(recipeToTest))
            {
                Debug.Log("Crafting succeeded.");
                craftingManager.Craft(recipeToTest);
            }
            else
            {
                Debug.Log("Crafting failed. Ingredients missing.");
            }
        }
    }
}


