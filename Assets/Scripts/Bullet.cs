using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float speed = 40.0f;
    public float maxDistance = 100.0f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        transform.Translate(speed * Vector3.forward);

        if (Vector3.Distance(startPosition, transform.position) > maxDistance)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
