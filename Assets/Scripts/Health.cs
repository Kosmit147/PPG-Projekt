using UnityEngine;

public class Health : MonoBehaviour
{
    public float health = 10.0f;
    public float maxHealth = 10.0f;
    public float healthRegenerationRate = 0.4f;

    void Update()
    {
        health = Mathf.Min(health + healthRegenerationRate * Time.deltaTime, maxHealth);
    }

    public void AddHealth(float value)
    {
        health = Mathf.Min(health + value, maxHealth);
    }

    public void RemoveHealth(float value)
    {
        health = Mathf.Max(health - value, 0);
    }
}
