using UnityEngine;

public class AddExperience : MonoBehaviour
{
    public float amount = 5.0f;
    public AudioClip soundEffect = null;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            player.PlayClip(soundEffect);
            
            if (other.TryGetComponent<Experience>(out var playerExperience))
                playerExperience.value += amount;

            Destroy(gameObject);
        }
    }
}
