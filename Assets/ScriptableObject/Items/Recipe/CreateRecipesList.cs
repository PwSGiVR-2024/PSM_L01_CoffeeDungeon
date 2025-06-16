using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateRecipesList : MonoBehaviour
{
    [MenuItem("Tools/Build Recipes List")]
    public static void BuildDatabase()
    {
        string[] guids = AssetDatabase.FindAssets("t:CraftingRecipe");
        List<CraftingRecipe> recipes = new();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            CraftingRecipe recipe = AssetDatabase.LoadAssetAtPath<CraftingRecipe>(path);
            if (recipe != null)
            {
                recipes.Add(recipe);
            }
        }

        RecipeList database = ScriptableObject.CreateInstance<RecipeList>();
        database.recipes = recipes;

        AssetDatabase.CreateAsset(database, "Assets/ScriptableObject/Items/Recipe/RecipeList.asset");
        AssetDatabase.SaveAssets();

        Debug.Log("Recipe List made successfully.");
    }
}
