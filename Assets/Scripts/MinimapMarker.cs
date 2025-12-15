using UnityEngine;

public class MinimapMarker : MonoBehaviour
{
    public GameObject target = null;
    public Vector3 offset = Vector3.zero;
    public bool matchYaw = false;

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = target.transform.position + offset;

        if (matchYaw)
        {
            var eulerAngles = transform.eulerAngles;
            transform.eulerAngles = new Vector3(eulerAngles.x, target.transform.eulerAngles.y, eulerAngles.z);
        }
    }
}
