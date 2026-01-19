using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> container = new();
    public int slots = 9;
    public int hotbarStartIndex = 6;

    public event Action OnInventoryChange;

    void Awake()
    {
        for (int i = container.Count; i < slots; i++)
            container.Add(new InventorySlot());
    }

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
        {
            for (int i = 0; i < container.Count; i++)
            {
                if (container[i].item == null)
                {
                    container[i] = new InventorySlot(item, amount);
                    break;
                }
            }
        }

        OnInventoryChange?.Invoke();
    }

    public void SwapItems(int indexA, int indexB)
    {
        if (indexA < 0 || indexA > container.Count || indexB < 0 || indexB > container.Count)
            return;

        (container[indexB], container[indexA]) = (container[indexA], container[indexB]);

        OnInventoryChange?.Invoke();
    }
}

[System.Serializable]
public class InventorySlot
{
    public ItemData item = null;
    public int amount = 0;

    public InventorySlot() { }

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
