using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthBarUI : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;

    [SerializeField] private List<Image> heartsList = new();

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found with tag 'Player'");
            return;
        }

        if (!player.TryGetComponent<PlayerController>(out playerController))
        {
            Debug.LogError("PlayerController component not found on player");
            return;
        }

        playerController.LoseHeart += LoseHeart;
        playerController.RestoreHearts += RestoreAllHearts;

        UpdateHeartsDisplay(5);
    }

    private void OnDisable()
    {
        if (playerController != null)
        {
            playerController.LoseHeart -= LoseHeart;
            playerController.RestoreHearts -= RestoreAllHearts;
        }
    }

    private void LoseHeart()
    {
        int currentHealth = playerController.GetHealth();
        UpdateHeartsDisplay(currentHealth);
    }

    private void RestoreAllHearts()
    {
        int maxHealth = playerController.GetMaxHealth();
        UpdateHeartsDisplay(maxHealth);
    }

    private void UpdateHeartsDisplay(int healthAmount)
    {
        for (int i = 0; i < heartsList.Count; i++)
        {
            if (i < healthAmount)
            {
                heartsList[i].gameObject.SetActive(true);
            }
            else
            {
                heartsList[i].gameObject.SetActive(false);
            }
        }
    }
}