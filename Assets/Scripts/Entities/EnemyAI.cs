using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    private NavMeshAgent enemyNMeshAgent;
    private GameObject player;
    private Coroutine patrolRoutine;

    private bool isHostile = false;
    [SerializeField] private float changeDirectionTime = 5f;
    [SerializeField] private float enemyDistance=1000;

    
    private void Start()
    {
        enemyNMeshAgent = GetComponent<NavMeshAgent>();

        if(enemyNMeshAgent == null)
        {
            print("no navmesh agent found");
        }

        player = GameObject.FindGameObjectWithTag("Player");
        patrolRoutine = StartCoroutine(PatrolRoutine());
    }

    private void Update()
    {
            if (isHostile)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        enemyNMeshAgent.SetDestination(player.transform.position);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isHostile)
        {
            isHostile = true;
            if (patrolRoutine != null)
            {
                StopCoroutine(patrolRoutine);
                patrolRoutine = null;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isHostile = false;
            patrolRoutine ??= StartCoroutine(PatrolRoutine());
        }
    }
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            print("patrol started");
            enemyNMeshAgent.SetDestination(RandomNavSphere(transform.position, enemyDistance, -1));
            yield return new WaitForSeconds(changeDirectionTime);
        }
    }
}
