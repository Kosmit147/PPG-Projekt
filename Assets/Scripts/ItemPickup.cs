using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData itemData;
    public int amount = 1;
    public AudioClip pickupSound = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            player.PlayClip(pickupSound);

            other.GetComponent<Inventory>().AddItem(itemData, amount);
            Destroy(gameObject);
        }
    }
}
