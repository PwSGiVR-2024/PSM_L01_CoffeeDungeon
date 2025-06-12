using System;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform weaponPoint;
    [SerializeField] private float attackRange = 1f;
    private int health = 5;
    private LayerMask enemyLayer;


    private bool isFightEnabled=false;
    private GameObject[] wallsToDisable;
    [SerializeField] private GameObject playerWeapon;
    private GameObject respawnPoint;
    private Vector3 respawn;
    private GameObject tray;

    private PlayerControls controls;

    private bool isInventoryOpen = false;
    private bool isCraftingOpen = false;

    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject craftingUI;
    void Start()
    {
        wallsToDisable = GameObject.FindGameObjectsWithTag("BackWalls");
        enemyLayer = LayerMask.GetMask("EnemyLayer");
        weaponPoint = GameObject.Find("swordTip").transform;
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        tray = GameObject.Find("Tray");

        controls = new PlayerControls();
        controls.Enable();
        controls.Gameplay.Attack.performed += ctx =>
        {
            if (isFightEnabled)
            {
                DealDamage();
            }
        };
        controls.Gameplay.OpenInventory.performed += ctx =>
        {
           
            if (!isInventoryOpen)
            {
                inventoryUI.SetActive(true);
                Time.timeScale = 0f;
                isInventoryOpen = true;
            }
            else
            {
                inventoryUI.SetActive(false);
                Time.timeScale = 1f;
                isInventoryOpen = false;
            }
        };

        controls.Gameplay.CraftingMenu.performed += ctx =>
        {
            if (!isCraftingOpen)
            {
                craftingUI.SetActive(true);
                Time.timeScale = 0f;
                isCraftingOpen = true;
            }
            else
            {
                craftingUI.SetActive(false);
                Time.timeScale = 1f;
                isCraftingOpen = false;
            }
        };
    }

    private void DealDamage()
    {

        Collider[] hitEnemies = Physics.OverlapSphere(weaponPoint.position, attackRange, enemyLayer);

        if (hitEnemies.Length > 0)
        {
            Collider closestEnemy = hitEnemies
                .OrderBy(e => Vector3.Distance(e.transform.position, weaponPoint.position))
                .First();

            if (closestEnemy.TryGetComponent<EnemyController>(out var ec))
            {
                ec.Die();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (weaponPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponPoint.position, attackRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FightArena"))
        {
            isFightEnabled = true;
            foreach(GameObject wall in wallsToDisable)
            {
                wall.SetActive(false);
            }
            playerWeapon.SetActive(true);
            tray.SetActive(false);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FightArena"))
        {
            isFightEnabled = false;
            foreach (GameObject wall in wallsToDisable)
            {
                wall.SetActive(true);
            }
            playerWeapon.SetActive(false);
            tray.SetActive(true);
            health = 5;
        }
    }

    public void TakeDamage()
    {
        if (health > 0)
        {
            health -= 1;
        }

        if (health == 0)
        {
            Fallen();
        }
    }

    private void Fallen()
    {
        respawn=respawnPoint.transform.position;
        gameObject.transform.position = respawn;
    }

}
