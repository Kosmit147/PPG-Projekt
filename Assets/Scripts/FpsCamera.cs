using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class FpsCamera : MonoBehaviour
{
    public InputActionProperty lookAction; // Expects a Vector2.
    public InputActionProperty zoomAction; // Expects a Vector2.

    public float lookSpeed = 0.1f;
    public float zoomSpeed = 1.0f;
    public float minPitch = -89.0f;
    public float maxPitch = 89.0f;

    private new Camera camera = null;
    private Vector2 eulerAngles = Vector2.zero;

    void Start()
    {
        eulerAngles = transform.eulerAngles;
        camera = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        var zoom = zoomSpeed * zoomAction.action.ReadValue<Vector2>();
        camera.fieldOfView -= zoom.y;

        var look = lookSpeed * lookAction.action.ReadValue<Vector2>();
        eulerAngles.x -= look.y;
        eulerAngles.y += look.x;
        eulerAngles.x = Mathf.Clamp(eulerAngles.x, -maxPitch, -minPitch);

        ApplyEulerAngles(eulerAngles);
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void ApplyEulerAngles(Vector2 eulerAngles)
    {
        var rotationX = Quaternion.AngleAxis(eulerAngles.x, Vector3.right);
        var rotationY = Quaternion.AngleAxis(eulerAngles.y, Vector3.up);
        transform.rotation = (rotationY * rotationX).normalized;
    }
}
