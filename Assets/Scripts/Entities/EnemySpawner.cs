using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject[] spawnPoints;
    private Transform[] spawnPointsLocation;

    [SerializeField] private float spawnInterval = 5f;
    private int maxEnemies = 10;
    public int currentEnemyCount = 0;

    private GameObject player;
    private PlayerController playerController;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        spawnPoints = GameObject.FindGameObjectsWithTag("MonsterSpawnPoint");
        spawnPointsLocation = new Transform[spawnPoints.Length];

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPointsLocation[i] = spawnPoints[i].transform;
        }

        InvokeRepeating(nameof(SpawnEnemy), 2f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount >= maxEnemies) return;

        int index = Random.Range(0, spawnPointsLocation.Length);
        Transform spawnPoint = spawnPointsLocation[index];

        GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        newEnemy.layer = LayerMask.NameToLayer("EnemyLayer");
        currentEnemyCount++;
    }

}
