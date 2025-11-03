using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    public InputActionProperty switchCameraAction; // Expects a button.

    public GameObject[] cameras = null;
    public GameObject CurrentCameraObject { get; private set; } = null;
    public Camera CurrentCamera { get; private set; } = null;

    private int currentCameraIdx = 0;

    void Start()
    {
        foreach (var camera in cameras)
            camera.SetActive(false);

        SetCamera(0);
    }

    void Update()
    {
        if (switchCameraAction.action.WasPerformedThisFrame())
            SwitchCamera();
    }

    void SwitchCamera()
    {
        var nextCameraIdx = (currentCameraIdx + 1) % cameras.Length;
        SetCamera(nextCameraIdx);
    }

    void SetCamera(int cameraIdx)
    {
        var prevCamera = CurrentCameraObject;

        CurrentCameraObject = cameras[cameraIdx];
        CurrentCamera = CurrentCameraObject.GetComponent<Camera>();
        currentCameraIdx = cameraIdx;

        if (prevCamera != null)
            prevCamera.SetActive(false);

        CurrentCameraObject.SetActive(true);
    }
}
