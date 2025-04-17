using System;
using UnityEngine;

public class PlayerController : EntityBaseClass
{

    private bool isFightEnabled=false;
    private GameObject[] wallsToDisable;

   // public Action inEnemyRange;

    void Start()
    {
        wallsToDisable = GameObject.FindGameObjectsWithTag("BackWalls");
    }

    void Update()
    {
        
    }

    protected override void DealDamage()
    {
        
    }
    protected override void TakeDamage()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FightArena"))
        {
            isFightEnabled = true;
            print(isFightEnabled);
            foreach(GameObject wall in wallsToDisable)
            {
                wall.SetActive(false);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FightArena"))
        {
            isFightEnabled = false;
            print(isFightEnabled);
            foreach (GameObject wall in wallsToDisable)
            {
                wall.SetActive(true);
            }

        }
        health = 5;
    }

}
