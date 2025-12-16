using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public float damage = 2.0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<Player>();
            player.Damage(damage * Time.deltaTime);
        }
    }
}
