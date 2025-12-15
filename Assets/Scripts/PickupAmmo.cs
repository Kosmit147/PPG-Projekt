using UnityEngine;

public class PickupAmmo : MonoBehaviour
{
    public int amount = 1;
    public bool destroyOnPickup = true;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var gunSwitcher = other.gameObject.GetComponent<GunSwitcher>();
            var gun = gunSwitcher.currentGun;

            var flareGun = gun.GetComponent<FlareGun>();

            if (flareGun)
                flareGun.AddAmmo(amount);

            if (destroyOnPickup)
                Destroy(gameObject);
        }
    }
}
