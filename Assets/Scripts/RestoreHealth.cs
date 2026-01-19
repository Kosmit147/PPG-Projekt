using UnityEngine;

public class RestoreHealth : MonoBehaviour
{
    public float health = 2.0f;
    public AudioClip soundEffect = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            player.PlayClip(soundEffect);
            
            if (other.TryGetComponent<Health>(out var playerHealth))
                playerHealth.AddHealth(health);

            Destroy(gameObject);
        }
    }
}
