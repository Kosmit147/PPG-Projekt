using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class Player : MonoBehaviour
{
    public InputActionProperty moveAction;               // Expects a Vector2.
    public InputActionProperty jumpAction;               // Expects a button.
    public InputActionProperty sprintAction;             // Expects a button.
    public InputActionProperty pointAction;              // Expects a Vector2.
    public InputActionProperty moveTowardsPointerAction; // Expects a button.
    public InputActionProperty switchControlModeAction;  // Expects a button.

    public float moveSpeed = 2.0f;
    public float sprintSpeed = 5.0f;
    public float jumpForce = 5.0f;

    public float gravity = -9.81f;

    [System.Serializable]
    public enum PlayerControlMode
    {
        Fps,
        TopDown,
    }

    public PlayerControlMode ControlMode
    {
        get => controlMode; set
        {
            switch (value)
            {
                case PlayerControlMode.Fps:
                    SetFpsControlMode();
                    break;
                case PlayerControlMode.TopDown:
                    SetTopDownControlMode();
                    break;
            }
        }
    }

    public PlayerControlMode initialControlMode = PlayerControlMode.Fps;

    public FpsCamera fpsCamera = null;
    public TopDownCamera topDownCamera = null;

    public SkinnedMeshRenderer armatureMeshRenderer = null;

    [System.Serializable]
    private struct AnimationParamIds
    {
        public int speed;
        public int jump;
        public int grounded;
        public int freeFall;
        public int motionSpeed;
    }

    private CharacterController characterController = null;
    private float verticalVelocity = 0.0f;

    private Animator animator = null;
    private AnimationParamIds animParams = new();

    private PlayerControlMode controlMode = PlayerControlMode.Fps;
    private Camera currentCamera = null;

    private Nullable<Vector3> moveTarget = null;

    void Start()
    {
        ControlMode = initialControlMode;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        RetrieveAnimParamIds();
        animator.SetFloat(animParams.motionSpeed, 1.0f);
    }

    void Update()
    {
        if (switchControlModeAction.action.WasPerformedThisFrame())
            SwitchControlMode();

        GroundAndGravityUpdate();

        switch (ControlMode)
        {
            case PlayerControlMode.Fps:
                FpsUpdate();
                break;
            case PlayerControlMode.TopDown:
                TopDownUpdate();
                break;
        }
    }

    void FpsUpdate()
    {
        var moveInput = Vector2.ClampMagnitude(moveAction.action.ReadValue<Vector2>(), 1.0f);
        var moveRotation = Quaternion.AngleAxis(currentCamera.transform.eulerAngles.y, Vector3.up);
        var rotatedMove = moveRotation * new Vector3(moveInput.x, 0.0f, moveInput.y);

        var motion = rotatedMove * GetFpsMotionSpeed();
        motion.y = verticalVelocity;
        var characterSpeed = new Vector2(motion.x, motion.z).magnitude;

        var forward = rotatedMove.normalized;

        if (forward != Vector3.zero)
            transform.forward = forward;

        characterController.Move(Time.deltaTime * motion);
        animator.SetFloat(animParams.speed, characterSpeed);
    }

    void TopDownUpdate()
    {
        if (moveTowardsPointerAction.action.WasPerformedThisFrame())
            SetMoveTarget();

        if (moveTarget == null)
            return;

        var moveEndPoint = new Vector2(moveTarget.Value.x, moveTarget.Value.z);
        var moveStartPoint = new Vector2(transform.position.x, transform.position.z);
        var move = moveEndPoint - moveStartPoint;
        move = Vector2.ClampMagnitude(move, sprintSpeed);
        var moveDirection = move.normalized;

        var motion = new Vector3(move.x, verticalVelocity, move.y);
        var characterSpeed = new Vector2(motion.x, motion.z).magnitude;
        var forward = moveDirection;

        if (forward != Vector2.zero)
            transform.forward = new Vector3(forward.x, 0.0f, forward.y);

        characterController.Move(Time.deltaTime * motion);
        animator.SetFloat(animParams.speed, characterSpeed);
    }

    void GroundAndGravityUpdate()
    {
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
            // We're free-falling.

            animator.SetBool(animParams.grounded, false);
            animator.SetBool(animParams.freeFall, true);
        }

        verticalVelocity += Time.deltaTime * gravity;
    }

    float GetFpsMotionSpeed()
    {
        if (sprintAction.action.IsPressed())
            return sprintSpeed;
        else
            return moveSpeed;
    }

    float GetTopDownMotionSpeed()
    {
        return sprintSpeed;
    }

    void SetMoveTarget()
    {
        var screenPoint = pointAction.action.ReadValue<Vector2>();
        var ray = currentCamera.ScreenPointToRay(screenPoint);

        if (Physics.Raycast(ray, out RaycastHit hit))
            moveTarget = hit.point;
        else
            moveTarget = null;
    }

    void SwitchControlMode()
    {
        switch (ControlMode)
        {
            case PlayerControlMode.Fps:
                SetTopDownControlMode();
                break;
            case PlayerControlMode.TopDown:
                SetFpsControlMode();
                break;
        }
    }

    void SetFpsControlMode()
    {
        controlMode = PlayerControlMode.Fps;

        if (currentCamera != null)
            currentCamera.gameObject.SetActive(false);

        currentCamera = fpsCamera.GetComponent<Camera>();
        currentCamera.gameObject.SetActive(true);

        moveTarget = null;
        armatureMeshRenderer.enabled = false;
    }

    void SetTopDownControlMode()
    {
        controlMode = PlayerControlMode.TopDown;

        if (currentCamera != null)
            currentCamera.gameObject.SetActive(false);

        var resetCameraPosition = topDownCamera.transform.position;
        resetCameraPosition.x = transform.position.x;
        resetCameraPosition.z = transform.position.z;
        topDownCamera.transform.position = resetCameraPosition;

        currentCamera = topDownCamera.GetComponent<Camera>();
        currentCamera.gameObject.SetActive(true);

        armatureMeshRenderer.enabled = true;
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
}
