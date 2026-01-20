using UnityEngine;

public class Shop : MonoBehaviour
{
    public Player player;
    public FlareGun flareGun;

    public float ammoMoneyCost = 1.0f;
    public float healthXPCost = 5.0f;
    public float staminaXPCost = 5.0f;
    public float sprintXPCost = 5.0f;

    public void BuyAmmo()
    {
        var playerInventory = player.GetComponent<Inventory>();

        if (playerInventory.GetMoney() >= ammoMoneyCost)
            flareGun.ammo += 1;
    }

    public void BuyHealth()
    {
        var playerHealth = player.GetComponent<Health>();
        var playerXP = player.GetComponent<Experience>();

        if (playerXP.value >= healthXPCost)
        {
            playerHealth.maxValue += 10.0f;
            playerXP.value = Mathf.Max(playerXP.value - healthXPCost, 0.0f);
        }
    }

    public void BuyStamina()
    {
        var playerStamina = player.GetComponent<Stamina>();
        var playerXP = player.GetComponent<Experience>();

        if (playerXP.value >= staminaXPCost)
        {
            playerStamina.maxValue += 10.0f;
            playerXP.value = Mathf.Max(playerXP.value - staminaXPCost, 0.0f);
        }
    }

    public void BuySprint()
    {
        var currentSprintSpeed = player.sprintSpeed;
        var playerXP = player.GetComponent<Experience>();

        if (playerXP.value >= sprintXPCost)
        {
            player.sprintSpeed += 2.0f;
            playerXP.value = Mathf.Max(playerXP.value - sprintXPCost, 0.0f);
        }
    }
}
