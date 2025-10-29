using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FpsPlayer : MonoBehaviour
{
    public GameObject cameraObject = null;

    [Header("Input")]
    public InputActionProperty lookAction;
    public float moveSpeed = 0.1f;
    public float lookSpeed = 0.1f;
    public float minPitch = -89.0f;
    public float maxPitch = 89.0f;

    private float pitch = 0.0f;
    private float yaw = 0.0f;

    void Start()
    {
        var euler = transform.localEulerAngles;
        pitch = euler.x;
        yaw = euler.y;
    }

    void Update()
    {
        UpdatePitchYaw();

        transform.localRotation = Quaternion.AngleAxis(yaw, Vector3.up);

        if (!cameraObject)
            return;

        cameraObject.transform.localRotation = Quaternion.AngleAxis(pitch, Vector3.right);
    }

    void UpdatePitchYaw()
    {
        var look = lookSpeed * lookAction.action.ReadValue<Vector2>();
        pitch = Mathf.Clamp(pitch - look.y, -maxPitch, -minPitch);
        yaw += look.x;
    }
}
