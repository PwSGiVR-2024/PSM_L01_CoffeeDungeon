using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : EntityBaseClass
{
    [SerializeField] private float attackRate = 2f;
    private bool playerInRange = false;
    private GameObject player;
    private PlayerController playerController;
    //private PlayerController playerController;

    private Coroutine attackCoroutine;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered enemy range.");
            playerInRange = true;
            if (attackCoroutine == null)
                attackCoroutine = StartCoroutine(AttackPlayer());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left enemy range.");
            playerInRange = false;
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
                attackCoroutine = null;
            }
        }
    }

    private IEnumerator AttackPlayer()
    {
        while (playerInRange)
        {
            DealDamage();
            yield return new WaitForSeconds(attackRate);
        }
    }

    protected override void DealDamage()
    {
        if (player != null)
        {
            Debug.Log("Enemy attacks the player!");
            playerController.TakeDamage();
        }
    }
}
