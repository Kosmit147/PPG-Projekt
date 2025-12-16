using UnityEngine;

public class RestoreHealth : MonoBehaviour
{
    public float health = 2.0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            player.AddHealth(health);
            Destroy(gameObject);
        }
    }
}
