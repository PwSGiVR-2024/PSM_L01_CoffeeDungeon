using System;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform weaponPoint;
    [SerializeField] private float attackRange = 1f;
    private int health = 5;
    private LayerMask enemyLayer;

    private bool isFightEnabled = false;
    private GameObject[] wallsToDisable;
    private GameObject respawnPoint;
    private Vector3 respawn;
    private GameObject tray;

    private PlayerControls controls;

    private bool isInventoryOpen = false;
    private bool isCraftingOpen = false;

    [SerializeField] private float interactionRange = 5f;
    [SerializeField] private LayerMask guestLayer;

    [Header("References")]
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject craftingUI;
    [SerializeField] private GameObject playerWeapon;
    [SerializeField] private SelectedItemHolder selectedItemHolder;

    public event Action CanGiveOrder;
    public event Action LeftOrderTrigger;

    void Start()
    {
        wallsToDisable = GameObject.FindGameObjectsWithTag("BackWalls");
        enemyLayer = LayerMask.GetMask("EnemyLayer");
        weaponPoint = GameObject.Find("swordTip").transform;
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        tray = GameObject.Find("Tray");

        if (guestLayer == 0)
        {
            guestLayer = LayerMask.GetMask("Default");
        }

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
            if (isCraftingOpen)
            {
                craftingUI.SetActive(false);
                isCraftingOpen = false;
            }

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
            if (isInventoryOpen)
            {
                inventoryUI.SetActive(false);
                isInventoryOpen = false;
            }
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

        controls.Gameplay.Give.performed += ctx =>
        {
            TryInteractWithGuest();
        };
    }

    private void TryInteractWithGuest()
    {
        GameObject[] allGuests = GameObject.FindGameObjectsWithTag("NPC");
        Collider[] nearbyGuests = Physics.OverlapSphere(transform.position, interactionRange, guestLayer);

        if (nearbyGuests.Length == 0)
        {
            foreach (GameObject guestObj in allGuests)
            {
                float distance = Vector3.Distance(transform.position, guestObj.transform.position);
                if (distance <= interactionRange)
                {
                    Guest guest = guestObj.GetComponent<Guest>();
                    if (guest != null && guest.CanReceiveItem())
                    {
                        if (selectedItemHolder != null && selectedItemHolder.HasItemSelected())
                        {
                            ItemData selectedItem = selectedItemHolder.GetCurrentHeldItem();
                            guest.ReceiveItem(selectedItem);
                            selectedItemHolder.ClearSelectedItem();
                            return;
                        }
                        return;
                    }
                }
            }
            return;
        }

        Collider closestGuest = null;
        float closestDistance = float.MaxValue;

        foreach (Collider guestCollider in nearbyGuests)
        {
            float distance = Vector3.Distance(transform.position, guestCollider.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestGuest = guestCollider;
            }
        }

        if (closestGuest != null)
        {
            Guest guest = closestGuest.GetComponent<Guest>();
            if (guest != null && guest.CanReceiveItem())
            {
                if (selectedItemHolder != null && selectedItemHolder.HasItemSelected())
                {
                    ItemData selectedItem = selectedItemHolder.GetCurrentHeldItem();
                    guest.ReceiveItem(selectedItem);
                    selectedItemHolder.ClearSelectedItem();
                }
            }
        }
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

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FightArena"))
        {
            isFightEnabled = true;
            foreach (GameObject wall in wallsToDisable)
            {
                wall.SetActive(false);
            }
            playerWeapon.SetActive(true);
            tray.SetActive(false);
        }

        if (other.CompareTag("NPC"))
        {
            CanGiveOrder?.Invoke();
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

        if (other.CompareTag("NPC"))
        {
            LeftOrderTrigger?.Invoke();
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
        respawn = respawnPoint.transform.position;
        gameObject.transform.position = respawn;
    }
}
