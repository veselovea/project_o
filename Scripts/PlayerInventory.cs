using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    private Canvas inventoryInterface;

    public bool IsOpen { get; private set; } = false;
    public Action<GameResources, float> ResourceAmountChangedEvent { get; set; } 
    public Dictionary<GameResources, float> AmountOfGameResources { get; private set; }
    public InventorySlot[] InventorySlots { get; private set; }

    void Awake()
    {
        inventoryInterface = GetComponentInChildren<Canvas>();
        inventoryInterface.enabled = IsOpen;
        AmountOfGameResources = new Dictionary<GameResources, float>(GameResource.CountOfGameResources);
        InventorySlots = new InventorySlot[10];
        for (int i = 0; i < GameResource.CountOfGameResources; i++)
        {
            AmountOfGameResources.Add((GameResources)i, 0);
        }
    }

    public void OpenOrCloseInventory()
    {
        IsOpen = !IsOpen;
        inventoryInterface.enabled = IsOpen;
        if (IsOpen)
        {

        }
    }

    public void AddGameResource(GameResources resource, float amount)
    {
        AmountOfGameResources[resource] += amount;
        ResourceAmountChangedEvent?.Invoke(resource, AmountOfGameResources[resource]);
    }

    public bool TakeResource(GameResources resource, float amount)
    {
        if (AmountOfGameResources[resource] < amount)
            return false;
        AmountOfGameResources[resource] -= amount;
        ResourceAmountChangedEvent?.Invoke(resource, AmountOfGameResources[resource]);
        return true;
    }

    public bool AddNewItem(string name, Image img, GameObject gameObject)
    {
        bool isAdded = false;
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (InventorySlots[i] == null)
            {
                InventorySlot slot = new InventorySlot(i, name, img, gameObject);
                InventorySlots[i] = slot;
                isAdded = true;
                break;
            }
        }
        return isAdded;
    }

    public void DropItem(int index)
    {
        InventorySlots[index] = null;
    }

    public void SwapItems(int from, int to)
    {
        (InventorySlots[from], InventorySlots[to]) = (InventorySlots[to], InventorySlots[from]);
    }
}
