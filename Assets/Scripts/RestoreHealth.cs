using UnityEngine;

public class RestoreHealth : MonoBehaviour
{
    public float health = 2.0f;
    public AudioClip pickupSound = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            player.PlayClip(pickupSound);
            player.AddHealth(health);
            Destroy(gameObject);
        }
    }
}
