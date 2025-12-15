using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> container = new();

    public event Action OnInventoryChange;

    public void AddItem(ItemData item, int amount)
    {
        bool hasItem = false;

        foreach (var slot in container)
        {
            if (slot.item == item)
            {
                slot.AddAmount(amount);
                hasItem = true;
            }
        }

        if (!hasItem)
            container.Add(new InventorySlot(item, amount));

        OnInventoryChange?.Invoke();
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int amount;

    public InventorySlot(ItemData item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
