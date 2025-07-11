using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeList", menuName = "Scriptable Objects/RecipeList")]
public class RecipeList : ScriptableObject
{
    public List<CraftingRecipe> recipes;
}
