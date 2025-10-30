using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public GameObject playerCamera = null;

    [Header("Input")]
    public InputActionProperty moveAction; // Expects a Vector2.
    public InputActionProperty lookAction; // Expects a Vector2.
    public InputActionProperty jumpAction; // Expects a button.
    public float moveSpeed = 2.0f;
    public float lookSpeed = 0.1f;
    public float jumpForce = 5.0f;
    public float minPitch = -89.0f;
    public float maxPitch = 89.0f;

    public float gravity = -9.81f;

    private Vector2 cameraEulerAngles = Vector2.zero;
    private CharacterController characterController = null;
    private float verticalVelocity = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cameraEulerAngles = transform.eulerAngles;
        characterController = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        UpdateCamera();
        Move();
    }

    void Move()
    {
        // TODO: Make sure to set transform.forward;
        // TODO: Sprinting.

        UpdateVerticalVelocity();

        var moveInput = Vector2.ClampMagnitude(moveAction.action.ReadValue<Vector2>(), 1.0f);
        var moveRotation = Quaternion.AngleAxis(cameraEulerAngles.y, Vector3.up);

        var motion = moveRotation * new Vector3(moveInput.x, 0.0f, moveInput.y) * moveSpeed;
        motion.y = verticalVelocity;

        characterController.Move(Time.deltaTime * motion);
    }

    void UpdateVerticalVelocity()
    {
        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0.0f)
                verticalVelocity = -1.0f; // Apply a constant downward force so that the isGrounded check works correctly.

            if (jumpAction.action.triggered)
                verticalVelocity = jumpForce;
        }

        verticalVelocity += Time.deltaTime * gravity;
    }

    void UpdateCamera()
    {
        var look = lookSpeed * lookAction.action.ReadValue<Vector2>();

        cameraEulerAngles.x -= look.y;
        cameraEulerAngles.y += look.x;
        cameraEulerAngles.x = Mathf.Clamp(cameraEulerAngles.x, -maxPitch, -minPitch);

        var rotationX = Quaternion.AngleAxis(cameraEulerAngles.x, Vector3.right);
        var rotationY = Quaternion.AngleAxis(cameraEulerAngles.y, Vector3.up);

        playerCamera.transform.rotation = (rotationY * rotationX).normalized;
    }
}
