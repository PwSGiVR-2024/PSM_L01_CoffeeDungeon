using UnityEngine;

[CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "Scriptable Objects/EnemyData")]
public class EnemyScriptableObject : ScriptableObject
{
    public string enemyName;
    public GameObject dropPrefab;
}
