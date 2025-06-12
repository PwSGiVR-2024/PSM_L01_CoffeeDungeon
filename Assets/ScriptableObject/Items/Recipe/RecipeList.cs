using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeList", menuName = "Scriptable Objects/RecipeList")]
public class RecipeList : ScriptableObject
{
    public List<CraftingRecipe> recipes; //list of all of the recipes in the game
}
