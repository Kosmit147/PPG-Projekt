using UnityEngine;

public class RemoveHealth : MonoBehaviour
{
    public float damage = 2.0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Health>(out var playerHealth))
                playerHealth.RemoveHealth(damage * Time.deltaTime);
        }
    }
}
