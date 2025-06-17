using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public delegate void EnemyDeathDelegate();

    private GameObject spawner;
    private EnemySpawner spawnerComponent;
    
    private void Start()
    {
        spawner = GameObject.Find("EnemySpawner");
        spawnerComponent = spawner.GetComponent<EnemySpawner>();
    }
    public void Die()
    {
        if (TryGetComponent<EnemyAI>(out var ai))
        {
            ai.StopAI();
        }

        AttachedDataEnemy data = GetComponent<AttachedDataEnemy>();
        if (data != null && data.enemyData != null && data.enemyData.dropPrefab != null)
        {
            Vector3 dropPosition = GetComponent<NavMeshAgent>().nextPosition;

            Instantiate(
                data.enemyData.dropPrefab,
                dropPosition + new Vector3(0, 0.5f, 0),
                Quaternion.identity
            );
        }

        Destroy(gameObject);
        spawnerComponent.currentEnemyCount-=1;
    }
   
}
