using UnityEngine;

public class PickupAmmo : MonoBehaviour
{
    public int amount = 1;
    public bool destroyOnPickup = true;
    public AudioClip pickupSound = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            player.PlayClip(pickupSound);

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
