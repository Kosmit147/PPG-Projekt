using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Flare : MonoBehaviour
{
    public float force = 40.0f;
    public float lifetime = 5.0f;

    void Start()
    {
        Destroy(gameObject, lifetime);
        var rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * force);
    }
}
