using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public new GameObject camera = null;

    public InputActionProperty moveAction; // Expects a Vector2.
    public InputActionProperty jumpAction; // Expects a button.
    public InputActionProperty sprintAction; // Expects a button.
    public float moveSpeed = 2.0f;
    public float sprintSpeed = 5.0f;
    public float jumpForce = 5.0f;

    public float gravity = -9.81f;

    private PlayerCamera playerCamera = null;
    private CharacterController characterController = null;
    private float verticalVelocity = 0.0f;

    void Start()
    {
        playerCamera = camera.GetComponent<PlayerCamera>();
        characterController = gameObject.GetOrAddComponent<CharacterController>();
    }

    void Update()
    {
        // TODO: Make sure to set transform.forward;

        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0.0f)
                verticalVelocity = -1.0f; // Apply a constant downward force so that the isGrounded check works correctly.

            if (jumpAction.action.WasPerformedThisFrame())
                verticalVelocity = jumpForce;
        }

        verticalVelocity += Time.deltaTime * gravity;

        var moveInput = Vector2.ClampMagnitude(moveAction.action.ReadValue<Vector2>(), 1.0f);
        var moveRotation = Quaternion.AngleAxis(playerCamera.EulerAngles.y, Vector3.up);

        var motion = moveRotation * new Vector3(moveInput.x, 0.0f, moveInput.y) * GetMoveSpeed();
        motion.y = verticalVelocity;

        characterController.Move(Time.deltaTime * motion);
    }

    float GetMoveSpeed()
    {
        if (sprintAction.action.IsPressed())
            return sprintSpeed;
        else
            return moveSpeed;
    }
}
