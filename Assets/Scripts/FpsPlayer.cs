using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FpsPlayer : MonoBehaviour
{
    [Header("Input")]
    public InputActionProperty lookAction;
    public float lookSpeed = 0.1f;
    public float minPitch = -89.0f;
    public float maxPitch = 89.0f;

    private Vector2 eulerAngles = Vector2.zero;

    void Start()
    {
        eulerAngles = transform.eulerAngles;
    }

    void Update()
    {
        var look = lookSpeed * lookAction.action.ReadValue<Vector2>();

        eulerAngles.x = Mathf.Clamp(eulerAngles.x - look.y, -maxPitch, -minPitch);
        eulerAngles.y += look.x;

        var rotationX = Quaternion.AngleAxis(eulerAngles.x, Vector3.right);
        var rotationY = Quaternion.AngleAxis(eulerAngles.y, Vector3.up);

        transform.rotation = (rotationY * rotationX).normalized;
    }
}
