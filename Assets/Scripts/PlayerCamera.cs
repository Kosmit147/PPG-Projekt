using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public enum CameraType
{
    Fps,
    TopDown,
}

[System.Serializable]
public struct FpsCameraProperties
{
    public float lookSpeed;
    public float zoomSpeed;
    public float minPitch;
    public float maxPitch;
    public float fov;

    public FpsCameraProperties(float lookSpeed, float zoomSpeed, float minPitch, float maxPitch, float fov)
    {
        this.lookSpeed = lookSpeed;
        this.zoomSpeed = zoomSpeed;
        this.minPitch = minPitch;
        this.maxPitch = maxPitch;
        this.fov = fov;
    }
}

[System.Serializable]
public struct TopDownCameraProperties
{
    public Vector2 eulerAngles;
    public Vector3 offset;
    public float panSpeed;
    public int panSensitivity; // How close to the edge of the window should the cursor be in order to move the camera.
    public float zoomSpeed;
    public float fov;

    public TopDownCameraProperties(Vector2 eulerAngles, Vector3 offset, float panSpeed, int panSensitivity, float zoomSpeed, float fov)
    {
        this.eulerAngles = eulerAngles;
        this.offset = offset;
        this.panSpeed = panSpeed;
        this.panSensitivity = panSensitivity;
        this.zoomSpeed = zoomSpeed;
        this.fov = fov;
    }
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
            panOffset = Vector2.zero;
        }
    }

    [SerializeField]
    private CameraType cameraType = CameraType.Fps;

    public Vector2 EulerAngles
    {
        get
        {
            switch (cameraType)
            {
                case CameraType.Fps:
                    return fpsEulerAngles;
                case CameraType.TopDown:
                    return topDownProperties.eulerAngles;
            }

            return Vector2.zero;
        }
    }

    public InputActionProperty switchCameraAction;  // Expects a button.
    public InputActionProperty lookAction;          // Expects a Vector2.
    public InputActionProperty zoomAction;          // Expects a Vector2.
    public InputActionProperty pointAction;         // Expects a Vector2.

    public FpsCameraProperties fpsProperties = new(0.1f, 1.0f, -89.0f, 89.0f, 60.0f);
    public TopDownCameraProperties topDownProperties = new(new Vector2(75.0f, 0.0f), new Vector3(0.0f, 20.0f, 0.0f), 5.0f, 10, 1.0f, 60.0f);

    private new Camera camera = null;
    private Vector2 fpsEulerAngles = Vector2.zero;
    private Vector2 panOffset = Vector2.zero;

    void Awake()
    {
        camera = gameObject.GetOrAddComponent<Camera>();
    }

    void Start()
    {
        UpdateCursorLockState();
        fpsEulerAngles = transform.eulerAngles;
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
        var zoom = fpsProperties.zoomSpeed * zoomAction.action.ReadValue<Vector2>();
        fpsProperties.fov -= zoom.y;

        var look = fpsProperties.lookSpeed * lookAction.action.ReadValue<Vector2>();
        fpsEulerAngles.x -= look.y;
        fpsEulerAngles.y += look.x;
        fpsEulerAngles.x = Mathf.Clamp(fpsEulerAngles.x, -fpsProperties.maxPitch, -fpsProperties.minPitch);

        transform.localPosition = Vector3.zero;
        ApplyEulerAngles(fpsEulerAngles);
        camera.fieldOfView = fpsProperties.fov;
    }

    void TopDownCameraUpdate()
    {
        var zoom = topDownProperties.zoomSpeed * zoomAction.action.ReadValue<Vector2>();
        topDownProperties.offset.y -= zoom.y;

        var pan = GetPan();
        var rotatedPan = Quaternion.AngleAxis(EulerAngles.y, Vector3.up) * new Vector3(pan.x, 0.0f, pan.y);
        panOffset.x += rotatedPan.x;
        panOffset.y += rotatedPan.z;

        transform.localPosition = topDownProperties.offset + new Vector3(panOffset.x, 0.0f, panOffset.y);
        ApplyEulerAngles(new Vector3(topDownProperties.eulerAngles.x, topDownProperties.eulerAngles.y, 0.0f));
        camera.fieldOfView = topDownProperties.fov;
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

    Vector2 GetPan()
    {
        var cursorPosition = pointAction.action.ReadValue<Vector2>();
        var pan = new Vector2(GetHorizontalPan(cursorPosition), GetVerticalPan(cursorPosition));
        return Time.deltaTime * topDownProperties.panSpeed * pan;
    }

    float GetHorizontalPan(Vector2 cursorPosition)
    {
        var panSensitivity = topDownProperties.panSensitivity;

        if (cursorPosition.x <= panSensitivity)
            return -1.0f;
        else if (cursorPosition.x >= Screen.width - panSensitivity)
            return 1.0f;

        return 0.0f;
    }

    float GetVerticalPan(Vector2 cursorPosition)
    {
        var panSensitivity = topDownProperties.panSensitivity;

        if (cursorPosition.y <= panSensitivity)
            return -1.0f;
        else if (cursorPosition.y >= Screen.height - panSensitivity)
            return 1.0f;

        return 0.0f;
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

    void ApplyEulerAngles(Vector2 eulerAngles)
    {
        var rotationX = Quaternion.AngleAxis(eulerAngles.x, Vector3.right);
        var rotationY = Quaternion.AngleAxis(eulerAngles.y, Vector3.up);
        transform.rotation = (rotationY * rotationX).normalized;
    }
}
