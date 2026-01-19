using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory playerInventory;
    public GameObject slotPrefab;
    public GameObject hotbarSlotPrefab;
    public GameObject inventoryDisplay;
    public GameObject hotbarDisplay;
    public FpsCamera fpsCamera;

    public InputActionProperty toggleAction; // Expects a button.

    private bool inventoryActive = false;

    void Start()
    {
        playerInventory.OnInventoryChange += UpdateUI;
        UpdateUI();
    }

    void Update()
    {
        if (toggleAction.action.WasPerformedThisFrame())
        {
            inventoryActive = !inventoryActive;
            inventoryDisplay.SetActive(inventoryActive);
            fpsCamera.enabled = !inventoryActive;

            if (inventoryActive)
                Cursor.lockState = CursorLockMode.Confined;
            else
                Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void UpdateUI()
    {
        foreach (Transform child in inventoryDisplay.transform)
            Destroy(child.gameObject);

        for (int i = 0; i < playerInventory.container.Count; i++)
        {
            var slot = playerInventory.container[i];

            GameObject newSlot = Instantiate(slotPrefab, inventoryDisplay.transform);
            var slotUI = newSlot.GetComponent<InventorySlotUI>();
            slotUI.slotIndex = i;
            slotUI.inventoryBackend = playerInventory;
            var item = newSlot.transform.Find("Item");

            if (slot.item != null)
            {
                Image iconDisplay = item.transform.Find("Icon").GetComponent<Image>();
                TextMeshProUGUI amountText = item.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI valueText = item.transform.Find("Value").GetComponent<TextMeshProUGUI>();

                iconDisplay.sprite = slot.item.icon;
                amountText.text = slot.amount.ToString();
                float itemValue = (float)slot.item.value * slot.amount / 100.0f;
                valueText.text = "$" + itemValue.ToString();
            }
            else
            {
                Image iconDisplay = item.transform.Find("Icon").GetComponent<Image>();
                TextMeshProUGUI amountText = item.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI valueText = item.transform.Find("Value").GetComponent<TextMeshProUGUI>();

                iconDisplay.color = new Color(0, 0, 0, 0);
                amountText.text = "";
                valueText.text = "";
            }
        }

        foreach (Transform child in hotbarDisplay.transform)
            Destroy(child.gameObject);

        for (int i = playerInventory.hotbarStartIndex; i < playerInventory.container.Count; i++)
        {
            var slot = playerInventory.container[i];

            GameObject newSlot = Instantiate(hotbarSlotPrefab, hotbarDisplay.transform);
            var item = newSlot.transform.Find("Item");

            if (slot.item != null)
            {
                Image iconDisplay = item.transform.Find("Icon").GetComponent<Image>();
                TextMeshProUGUI amountText = item.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI valueText = item.transform.Find("Value").GetComponent<TextMeshProUGUI>();

                iconDisplay.sprite = slot.item.icon;
                amountText.text = slot.amount.ToString();
                float itemValue = (float)slot.item.value * slot.amount / 100.0f;
                valueText.text = "$" + itemValue.ToString();
            }
            else
            {
                Image iconDisplay = item.transform.Find("Icon").GetComponent<Image>();
                TextMeshProUGUI amountText = item.transform.Find("Amount").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI valueText = item.transform.Find("Value").GetComponent<TextMeshProUGUI>();

                iconDisplay.color = new Color(0, 0, 0, 0);
                amountText.text = "";
                valueText.text = "";
            }
        }
    }
}
