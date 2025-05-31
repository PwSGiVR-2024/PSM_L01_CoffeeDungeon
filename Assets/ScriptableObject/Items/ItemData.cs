using UnityEngine;
public enum ItemType
{
    pastry,
    drink,
    monsterDrop
}

[CreateAssetMenu(fileName = "MonsterDrop", menuName = "Scriptable Objects/Items")]
public class ItemData : ScriptableObject
{
    public int id;
    public Sprite iconPrefab;
    public GameObject inGameObject;
    public string itemName;
    public ItemType itemType;
    public bool isCraftable; 
}
