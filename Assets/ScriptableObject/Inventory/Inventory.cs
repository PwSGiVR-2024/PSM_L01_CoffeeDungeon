using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Scriptable Objects/Inventory")]
public class Inventory : ScriptableObject
{
    public int inventoryCapacity = 99;
    public List<InventorySlot> slots = new();

    public event Action OnInventoryChanged;
    public event Action AmountChanged;
    public void AddItem(ItemData item, int amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item)
            {
                slot.AddAmount(amount);
                AmountChanged?.Invoke();
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

    public void RemoveItem(ItemData item, int amount)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == item)
            {
                if (slots[i].amount > amount)
                {
                    slots[i].amount -= amount;
                    AmountChanged?.Invoke();
                    return;
                }
                else
                {
                    amount -= slots[i].amount;
                    slots.RemoveAt(i);
                    OnInventoryChanged?.Invoke();
                    AmountChanged?.Invoke();
                    return;
                }
            }
        }

        Debug.LogWarning($"Tried to remove {amount} of {item.name}, but it wasn't found in inventory.");
    }

    public bool HasItem(ItemData item, int requiredAmount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == item && slot.amount >= requiredAmount)
                return true;
        }
        return false;
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