using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingRecipe", menuName = "Scriptable Objects/CraftingRecipe")]
public class CraftingRecipe : ScriptableObject
{
    [System.Serializable]
    public class Ingredient
    {
        public ItemData item;
        public int quantity;
    }

    public List<Ingredient> ingredients;
    public ItemData result;
    public int resultQuantity = 1;
}
