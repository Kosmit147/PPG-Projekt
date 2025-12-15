using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory playerInventory;
    public GameObject slotPrefab;
    public GameObject inventoryGrid;

    public InputActionProperty toggleAction; // Expects a button.

    void Start()
    {
        playerInventory.OnInventoryChange += UpdateUI;
        UpdateUI();
    }

    void Update()
    {
        if (toggleAction.action.WasPerformedThisFrame())
            inventoryGrid.SetActive(!inventoryGrid.activeSelf);
    }

    void UpdateUI()
    {
        foreach (Transform child in inventoryGrid.transform)
            Destroy(child.gameObject);

        foreach (var slot in playerInventory.container)
        {
            GameObject newSlot = Instantiate(slotPrefab, inventoryGrid.transform);

            Image iconDisplay = newSlot.transform.Find("Icon").GetComponent<Image>();
            TextMeshProUGUI amountText = newSlot.transform.Find("Amount").GetComponent<TextMeshProUGUI>();

            iconDisplay.sprite = slot.item.icon;
            amountText.text = slot.amount.ToString();
        }
    }
}
