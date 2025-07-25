using UnityEngine;
public enum ItemType
{
    pastry,
    drink,
    monsterDrop,
    ingredient
}

[CreateAssetMenu(fileName = "MonsterDrop", menuName = "Scriptable Objects/Items")]
public class ItemData : ScriptableObject
{
    public Sprite iconPrefab;
    public GameObject inGameObject;
    public string itemName;
    public ItemType itemType;
    public bool isCraftable;
    public bool hasSlime;
}
