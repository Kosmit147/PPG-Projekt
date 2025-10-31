using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CameraType
{
    Fps,
    TopDown,
}

public class PlayerCamera : MonoBehaviour
{
    public CameraType CameraType
    {
        get => cameraType;
        set
        {
            cameraType = value;
            UpdateCursorLockState();
        }
    }

    public Vector2 EulerAngles
    {
        get
        {
            switch (cameraType)
            {
                case CameraType.Fps:
                    return fpsEulerAngles;
                case CameraType.TopDown:
                    return topDownEulerAngles;
            }

            return Vector2.zero;
        }
    }

    public InputActionProperty switchCameraAction; // Expects a button.

    [Header("Fps Camera Properties")]
    public InputActionProperty lookAction; // Expects a Vector2.
    public float lookSpeed = 0.1f;
    public float minPitch = -89.0f;
    public float maxPitch = 89.0f;

    [Header("Top-down Camera Properties")]
    public float cameraPitch = 75.0f;
    public float cameraYaw = 0.0f;
    public Vector3 cameraOffset = new(0.0f, 20.0f, -3.0f);

    private CameraType cameraType = CameraType.Fps;
    private Vector2 fpsEulerAngles = Vector2.zero;
    private Vector2 topDownEulerAngles = Vector2.zero;

    void Awake()
    {
        gameObject.GetOrAddComponent<Camera>();
    }

    void Start()
    {
        UpdateCursorLockState();
        InitEulerAngles();
    }

    void Update()
    {
        if (switchCameraAction.action.WasPerformedThisFrame())
            SwitchCamera();

        switch (cameraType)
        {
            case CameraType.Fps:
                FpsCameraUpdate();
                break;
            case CameraType.TopDown:
                TopDownCameraUpdate();
                break;
        }
    }

    void FpsCameraUpdate()
    {
        var look = lookSpeed * lookAction.action.ReadValue<Vector2>();

        fpsEulerAngles.x -= look.y;
        fpsEulerAngles.y += look.x;
        fpsEulerAngles.x = Mathf.Clamp(fpsEulerAngles.x, -maxPitch, -minPitch);

        transform.localPosition = Vector3.zero;
        ApplyEulerAngles(fpsEulerAngles);
    }

    void TopDownCameraUpdate()
    {
        // TODO: Zoom.

        transform.localPosition = cameraOffset;
        topDownEulerAngles = new Vector3(cameraPitch, cameraYaw, 0.0f);
        ApplyEulerAngles(topDownEulerAngles);
    }

    void UpdateCursorLockState()
    {
        switch (cameraType)
        {
            case CameraType.Fps:
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case CameraType.TopDown:
                Cursor.lockState = CursorLockMode.Confined;
                break;
        }
    }

    void SwitchCamera()
    {
        switch (cameraType)
        {
            case CameraType.Fps:
                CameraType = CameraType.TopDown;
                break;
            case CameraType.TopDown:
                CameraType = CameraType.Fps;
                break;
        }
    }

    void InitEulerAngles()
    {
        fpsEulerAngles = transform.eulerAngles;
        topDownEulerAngles = new Vector2(cameraPitch, cameraYaw);
    }

    void ApplyEulerAngles(Vector2 eulerAngles)
    {
        var rotationX = Quaternion.AngleAxis(eulerAngles.x, Vector3.right);
        var rotationY = Quaternion.AngleAxis(eulerAngles.y, Vector3.up);
        transform.rotation = (rotationY * rotationX).normalized;
    }
}
