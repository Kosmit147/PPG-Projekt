using UnityEngine;

public class Stamina : MonoBehaviour
{
    public float value = 10.0f;
    public float maxValue = 10.0f;
    public float depletionRate = 0.0f;
    public float regenerationRate = 0.4f;

    void LateUpdate()
    {
        value = Mathf.Min(value + regenerationRate * Time.deltaTime, maxValue);
        value = Mathf.Max(value - depletionRate * Time.deltaTime, 0.0f);
    }
}
