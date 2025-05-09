using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDrop", menuName = "Scriptable Objects/MonsterDrop")]
public class MonsterDrop : ScriptableObject
{
    public GameObject iconPrefab;
    public GameObject inGameObject;
    public string itemName;
}
