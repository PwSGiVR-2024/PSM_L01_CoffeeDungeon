using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public delegate void EnemyDeathDelegate();

    private EnemySpawner spawner;
    //public event EnemyDeathDelegate OnEnemyDeath;

    private void Start()
    {
        spawner = GetComponent<EnemySpawner>();
    }
    public void Die()
    {
        EnemyAI ai = GetComponent<EnemyAI>();
        if (ai != null)
        {
            ai.StopAI();
        }

        AttachedDataEnemy data = GetComponent<AttachedDataEnemy>();
        if (data != null && data.enemyData != null && data.enemyData.dropInGameModel != null)
        {
            Vector3 dropPosition = GetComponent<NavMeshAgent>().nextPosition;

            Instantiate(
                data.enemyData.dropInGameModel,
                dropPosition + new Vector3(0, 0.5f, 0),
                Quaternion.identity
            );
        }

        Destroy(gameObject);
        spawner.currentEnemyCount--;
    }
   
}
