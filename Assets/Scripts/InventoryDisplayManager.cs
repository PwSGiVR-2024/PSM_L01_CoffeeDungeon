using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplayManager : MonoBehaviour
{
    public InventoryDisplayManager Instance { get; private set; }
    [SerializeField]private Inventory inventory;
    [SerializeField]private Transform slotContainer;
    [SerializeField]private GameObject slotPrefab;

    private readonly List<GameObject> slots = new();
    private void Awake()
    {
        if (Instance != null & Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        DrawInventory();
    }

    private void DrawInventory()
    {
        foreach (var slot in slots)
        {
            Destroy(slot);
        }
        slots.Clear();

        for (int i = 0; i < inventory.slots.Count; i++)
        {
            GameObject slotGo = Instantiate(slotPrefab, slotContainer);
            InventorySlotUI slotUI = slotGo.GetComponent<InventorySlotUI>();

            slotUI.slotIndex = i;
            slotUI.inventoryReference = inventory;
            slotUI.Setup(inventory.slots[i]);

            slots.Add(slotGo);
        }
    }
    private void OnEnable()
    {
        inventory.OnInventoryChanged += DrawInventory;
    }

    private void OnDisable()
    {
        inventory.OnInventoryChanged -= DrawInventory;
    }
}
