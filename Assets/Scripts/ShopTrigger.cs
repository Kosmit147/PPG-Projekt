using UnityEngine;
using UnityEngine.InputSystem;

public class ShopTrigger : MonoBehaviour
{
    public InputActionProperty openShopAction; // Expects a button.

    public GameObject shopUI;
    public GameObject inventoryUI;
    public GameObject moneyText;
    public GameObject interactMessage;

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
        }
    }

    void ToggleShop()
    {
        bool shopActive = shopUI.activeSelf;
        shopUI.SetActive(!shopActive);
        shopActive = shopUI.activeSelf;
        interactMessage.SetActive(!shopActive);
        inventoryUI.SetActive(shopActive);
        moneyText.SetActive(shopActive);

        if (shopActive)
            Cursor.lockState = CursorLockMode.Confined;
        else
            Cursor.lockState = CursorLockMode.Locked;
    }
}
