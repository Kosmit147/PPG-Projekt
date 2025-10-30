using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FpsCamera : MonoBehaviour
{
    public InputActionProperty lookAction; // Expects a Vector2.
    public float lookSpeed = 0.1f;
    public float minPitch = -89.0f;
    public float maxPitch = 89.0f;

    private Vector2 eulerAngles;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gameObject.GetOrAddComponent<Camera>();
    }

    void Update()
    {
        var look = lookSpeed * lookAction.action.ReadValue<Vector2>();

        eulerAngles.x -= look.y;
        eulerAngles.y += look.x;
        eulerAngles.x = Mathf.Clamp(eulerAngles.x, -maxPitch, -minPitch);

        var rotationX = Quaternion.AngleAxis(eulerAngles.x, Vector3.right);
        var rotationY = Quaternion.AngleAxis(eulerAngles.y, Vector3.up);

        transform.rotation = (rotationY * rotationX).normalized;
    }
}
