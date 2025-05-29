using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    public int inventoryCapacity = 99;
    public List<InventorySlot> container = new List<InventorySlot>();

    public System.Action OnInventoryChanged;
    public void AddItem(ItemData item, int amount)
    {
        foreach (InventorySlot slot in container)
        {
            if (slot.item == item)
            {
                slot.AddAmount(amount);
                return;
            }
        }

        if (container.Count < inventoryCapacity)
        {
            InventorySlot newSlot = new InventorySlot(item, amount);
            container.Add(newSlot);
        }
        else
        {
            Debug.LogWarning("Inventory is full! Cannot add item: " + item.name);
        }

        OnInventoryChanged?.Invoke();

    }

}

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int amount;

    public InventorySlot(ItemData itemInSlot, int howMany)
    {
        item = itemInSlot;
        amount = howMany;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}