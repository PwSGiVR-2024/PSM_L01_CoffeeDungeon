using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public class EnemyController : MonoBehaviour
{
    public delegate void EnemyDeathDelegate();

    private GameObject spawner;
    private EnemySpawner spawnerComponent;
    private AudioSource audioSource;

    [Header("References")]
    [SerializeField] private AudioClip dieSound;
    
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

        if (audioSource != null && dieSound != null)
        {
            audioSource.PlayOneShot(dieSound);
        }
        Destroy(gameObject, dieSound.length);
        spawnerComponent.currentEnemyCount-=1;
    }
   
}
