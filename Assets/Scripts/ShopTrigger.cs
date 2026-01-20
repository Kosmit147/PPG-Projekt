using UnityEngine;
using UnityEngine.InputSystem;

public class ShopTrigger : MonoBehaviour
{
    public InputActionProperty openShopAction; // Expects a button.

    public GameObject shopUI;
    public GameObject inventoryDisplay;
    public GameObject inventoryManager;
    public GameObject moneyText;
    public GameObject interactMessage;
    public FlareGun flareGun;
    public FpsCamera fpsCamera;

    private bool playerInZone = false;

    void Start()
    {
        shopUI.SetActive(false);
        interactMessage.SetActive(false);
    }

    void Update()
    {
        if (playerInZone)
        {
            if (openShopAction.action.WasPerformedThisFrame())
                ToggleShop();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            interactMessage.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            interactMessage.SetActive(false);
            shopUI.SetActive(false);
            flareGun.canShoot = true;
            fpsCamera.enabled = true;
            var inventoryUIComponent = inventoryManager.GetComponent<InventoryUI>();
            inventoryUIComponent.showSellButtons = false;
        }
    }

    void ToggleShop()
    {
        bool shopActive = shopUI.activeSelf;
        shopUI.SetActive(!shopActive);
        shopActive = shopUI.activeSelf;
        interactMessage.SetActive(!shopActive);
        inventoryDisplay.SetActive(shopActive);
        var inventoryUIComponent = inventoryManager.GetComponent<InventoryUI>();
        inventoryUIComponent.showSellButtons = shopActive;
        inventoryUIComponent.UpdateUI();
        moneyText.SetActive(shopActive);
        flareGun.canShoot = !shopActive;
        fpsCamera.enabled = !shopActive;

        if (shopActive)
            Cursor.lockState = CursorLockMode.Confined;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
}
