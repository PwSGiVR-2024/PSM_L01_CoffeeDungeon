using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guest : MonoBehaviour
{
    public ItemData firstChoice;
    public ItemData secondChoice;

    [SerializeField] private Image firstChoiceImage;
    [SerializeField] private Image secondChoiceImage;


    [SerializeField] private CraftableItemList itemList;


    private void Awake()
    {
        GenerateFirstSecondChoice();
    }

    private void GenerateFirstSecondChoice()
    {
        if (itemList == null || itemList.craftableItems.Count == 0)
        {
            Debug.LogWarning("Craftable item list is empty or not assigned!");
            return;
        }

        int index1 = Random.Range(0, itemList.craftableItems.Count);
        firstChoice = itemList.craftableItems[index1];

        int index2;
        do
        {
            index2 = Random.Range(0, itemList.craftableItems.Count);
        } while (index2 == index1);

        secondChoice = itemList.craftableItems[index2];
        firstChoiceImage.sprite = itemList.craftableItems[index1].iconPrefab;
        secondChoiceImage.sprite = itemList.craftableItems[index2].iconPrefab;
    }

    private void Spawn()
    {

    }

    private void RateOrder()
    {

    }

    private void Pay()
    {

    }

}
