using UnityEngine;
using UnityEngine.InputSystem;

struct AnimationParamIds
{
    public int speed;
    public int jump;
    public int grounded;
    public int freeFall;
    public int motionSpeed;
}

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class Player : MonoBehaviour
{
    public GameObject[] cameras = null;

    public InputActionProperty moveAction;   // Expects a Vector2.
    public InputActionProperty jumpAction;   // Expects a button.
    public InputActionProperty sprintAction; // Expects a button.
    public InputActionProperty switchCameraAction;  // Expects a button.
    public float moveSpeed = 2.0f;
    public float sprintSpeed = 5.0f;
    public float jumpForce = 5.0f;

    public float gravity = -9.81f;

    private GameObject currentCamera = null;
    private uint currentCameraIdx = 0;
    private CharacterController characterController = null;
    private float verticalVelocity = 0.0f;

    private Animator animator = null;
    private AnimationParamIds animParams = new();

    void Start()
    {
        if (cameras != null && cameras.Length != 0)
        {
            currentCamera = cameras[0];
            currentCameraIdx = 0;
        }
        else if (currentCamera == null)
        {
            currentCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        RetrieveAnimParamIds();
        animator.SetFloat(animParams.motionSpeed, 1.0f);
    }

    void Update()
    {
        if (switchCameraAction.action.WasPerformedThisFrame())
            SwitchCamera();

        if (characterController.isGrounded)
        {
            if (verticalVelocity < 0.0f)
                verticalVelocity = -1.0f; // Apply a constant downward force so that the isGrounded check works correctly.

            if (jumpAction.action.WasPerformedThisFrame())
            {
                verticalVelocity = jumpForce;
                animator.SetBool(animParams.jump, true);
            }
            else
            {
                animator.SetBool(animParams.jump, false);
            }

            animator.SetBool(animParams.grounded, true);
            animator.SetBool(animParams.freeFall, false);
        }
        else if (verticalVelocity < 0.0f)
        {
            // We're free falling.

            animator.SetBool(animParams.grounded, false);
            animator.SetBool(animParams.freeFall, true);
        }

        verticalVelocity += Time.deltaTime * gravity;

        var moveInput = Vector2.ClampMagnitude(moveAction.action.ReadValue<Vector2>(), 1.0f);
        var moveRotation = Quaternion.AngleAxis(currentCamera.transform.eulerAngles.y, Vector3.up);
        var rotatedMove = moveRotation * new Vector3(moveInput.x, 0.0f, moveInput.y);

        var motion = rotatedMove * GetMotionSpeed();
        motion.y = verticalVelocity;
        var characterSpeed = new Vector2(motion.x, motion.z).magnitude;

        var forward = rotatedMove.normalized;

        if (forward != Vector3.zero)
            transform.forward = forward;

        characterController.Move(Time.deltaTime * motion);
        animator.SetFloat(animParams.speed, characterSpeed);
    }

    float GetMotionSpeed()
    {
        if (sprintAction.action.IsPressed())
            return sprintSpeed;
        else
            return moveSpeed;
    }

    void OnFootstep(AnimationEvent animationEvent)
    {
        // TODO: Implement.
    }

    void OnLand(AnimationEvent animationEvent)
    {
        // TODO: Implement.
    }

    void RetrieveAnimParamIds()
    {
        animParams = new()
        {
            speed = Animator.StringToHash("Speed"),
            jump = Animator.StringToHash("Jump"),
            grounded = Animator.StringToHash("Grounded"),
            freeFall = Animator.StringToHash("FreeFall"),
            motionSpeed = Animator.StringToHash("MotionSpeed")
        };
    }

    void SetCamera(uint cameraIdx)
    {
        var prevCamera = currentCamera;

        currentCamera = cameras[cameraIdx];
        currentCameraIdx = cameraIdx;

        prevCamera.SetActive(false);
        currentCamera.SetActive(true);
    }

    void SwitchCamera()
    {
        uint nextCameraIdx = (uint)((currentCameraIdx + 1) % cameras.Length);
        SetCamera(nextCameraIdx);
    }
}
