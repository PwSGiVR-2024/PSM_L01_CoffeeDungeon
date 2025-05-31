using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Rendering;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    public int inventoryCapacity = 99;
    public List<InventorySlot> slots = new List<InventorySlot>();

    public System.Action OnInventoryChanged;
    public void AddItem(ItemData item, int amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item)
            {
                slot.AddAmount(amount);
                return;
            }
        }

        if (slots.Count < inventoryCapacity)
        {
            InventorySlot newSlot = new(item, amount);
            slots.Add(newSlot);
        }
        else
        {
            Debug.LogWarning("Inventory is full! Cannot add item: " + item.name);
        }

        OnInventoryChanged?.Invoke();

    }
    public void RemoveSlot(int index)
    {
        if (index >= 0 && index < slots.Count)
        {
            slots.RemoveAt(index);
            OnInventoryChanged?.Invoke();
        }
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

    public void Clear()
    {
        item = null;
        amount = 0;
    }
}