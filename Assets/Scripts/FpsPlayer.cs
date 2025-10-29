using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FpsPlayer : MonoBehaviour
{
    public GameObject cameraObject = null;

    [Header("Input")]
    public InputActionProperty moveAction;
    public InputActionProperty lookAction;
    public float moveSpeed = 1000.0f;
    public float lookSpeed = 0.1f;
    public float minPitch = -89.0f;
    public float maxPitch = 89.0f;

    private float pitch = 0.0f;
    private float yaw = 0.0f;
    private CharacterController characterController = null;

    void Start()
    {
        characterController = gameObject.GetComponent<CharacterController>();

        var euler = transform.localEulerAngles;
        pitch = euler.x;
        yaw = euler.y;
    }

    void Update()
    {
        UpdatePitchYaw();

        transform.localRotation = Quaternion.AngleAxis(yaw, Vector3.up);
        cameraObject.transform.localRotation = Quaternion.AngleAxis(pitch, Vector3.right);

        Move();
    }

    void UpdatePitchYaw()
    {
        var look = lookSpeed * lookAction.action.ReadValue<Vector2>();
        pitch = Mathf.Clamp(pitch - look.y, -maxPitch, -minPitch);
        yaw += look.x;
    }

    void Move()
    {
        var move = moveSpeed * Time.deltaTime * moveAction.action.ReadValue<Vector2>();
        characterController.SimpleMove(transform.localRotation * new Vector3(move.x, 0.0f, move.y));
    }
}
