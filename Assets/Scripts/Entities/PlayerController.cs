using System;
using UnityEngine;

public class PlayerController : EntityBaseClass
{
    private Transform weaponPoint;
    [SerializeField] private float attackRange = 1f;
    private LayerMask enemyLayer;


    private bool isFightEnabled=false;
    private GameObject[] wallsToDisable;
    [SerializeField]private GameObject playerWeapon;
    private GameObject respawnPoint;
    private Vector3 respawn;

    private PlayerControls controls;

    //public Action canBeAttacked;

    // public Action inEnemyRange;

    void Start()
    {
        wallsToDisable = GameObject.FindGameObjectsWithTag("BackWalls");
        enemyLayer = LayerMask.GetMask("EnemyLayer");
        weaponPoint = GameObject.Find("swordTip").transform;
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn");


        health = 5;


        controls = new PlayerControls();
        controls.Enable();
        controls.Gameplay.Attack.performed += ctx =>
        {
            if (isFightEnabled)
            {
                DealDamage();
            }
        };
    }

    protected override void DealDamage()
    {
        Debug.Log("Player attacked");

        Collider[] hitEnemies = Physics.OverlapSphere(weaponPoint.position, attackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (weaponPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weaponPoint.position, attackRange);
    }

    //private void OnDrawGizmosSelected()
    //{
    //    if (weaponPoint == null) return;
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(weaponPoint.position, attackRange);
    //}

    //protected override void TakeDamage()
    //{

    //}
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
            health = 5;
        }
    }

    public void TakeDamage()
    {
        if (health > 0)
        {
            health -= 1;
            print(health);
        }

        if (health == 0)
        {
            Fallen();
        }
    }

    protected override void Fallen()
    {
        respawn=respawnPoint.transform.position;
        gameObject.transform.position = respawn;
    }

}
