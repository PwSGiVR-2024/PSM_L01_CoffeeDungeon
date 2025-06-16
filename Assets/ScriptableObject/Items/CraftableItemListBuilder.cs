using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CraftableItemListBuilder : MonoBehaviour
{
    [MenuItem("Tools/Build Craftable Item List")]
    public static void BuildDatabase()
    {
        string[] guids = AssetDatabase.FindAssets("t:ItemData");
        List<ItemData> craftableItems = new();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ItemData item = AssetDatabase.LoadAssetAtPath<ItemData>(path);
            if (item != null && item.isCraftable)
            {
                craftableItems.Add(item);
            }
        }

        CraftableItemList database = ScriptableObject.CreateInstance<CraftableItemList>();
        database.craftableItems = craftableItems;

        AssetDatabase.CreateAsset(database, "Assets/ScriptableObject/Items/CraftableItemList.asset");
        AssetDatabase.SaveAssets();

        Debug.Log("Craftable Item List made successfully.");
    }
}
