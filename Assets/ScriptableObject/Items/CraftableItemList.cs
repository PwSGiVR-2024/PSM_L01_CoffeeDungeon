using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftableItemList", menuName = "Scriptable Objects/CraftableItemList")]
public class CraftableItemList : ScriptableObject
{
        public List<ItemData> craftableItems;
}
