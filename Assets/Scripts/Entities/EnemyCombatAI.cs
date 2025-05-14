using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyCombatAI : MonoBehaviour
{
    [SerializeField] private float attackRate = 2f;
    private bool playerInRange = false;
    private GameObject player;
    private PlayerController playerController;

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
            playerInRange = true;
            attackCoroutine ??= StartCoroutine(AttackPlayer());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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

    private void DealDamage()
    {
        if (player != null)
        {
            playerController.TakeDamage();
        }
    }

}
