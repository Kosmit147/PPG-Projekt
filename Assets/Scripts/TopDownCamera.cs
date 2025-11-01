using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class TopDownCamera : MonoBehaviour
{
    public GameObject player = null;

    public float panSpeed = 15.0f;
    public int panSensitivity = 10; // How close to the edge of the window should the cursor be in order to move the camera.
    public float zoomSpeed = 1.0f;

    public InputActionProperty zoomAction;   // Expects a Vector2.
    public InputActionProperty pointAction;  // Expects a Vector2.

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        var zoom = zoomSpeed * zoomAction.action.ReadValue<Vector2>();
        transform.localPosition += new Vector3(0.0f, -zoom.y, 0.0f);

        var pan = GetPan();
        var rotatedPan = Quaternion.AngleAxis(transform.eulerAngles.y, Vector3.up) * new Vector3(pan.x, 0.0f, pan.y);
        transform.localPosition += new Vector3(rotatedPan.x, 0.0f, rotatedPan.z);
    }

    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    Vector2 GetPan()
    {
        var cursorPosition = pointAction.action.ReadValue<Vector2>();
        var pan = new Vector2(GetHorizontalPan(cursorPosition), GetVerticalPan(cursorPosition));
        return Time.deltaTime * panSpeed * pan;
    }

    float GetHorizontalPan(Vector2 cursorPosition)
    {
        if (cursorPosition.x <= panSensitivity)
            return -1.0f;
        else if (cursorPosition.x >= Screen.width - panSensitivity)
            return 1.0f;

        return 0.0f;
    }

    float GetVerticalPan(Vector2 cursorPosition)
    {
        if (cursorPosition.y <= panSensitivity)
            return -1.0f;
        else if (cursorPosition.y >= Screen.height - panSensitivity)
            return 1.0f;

        return 0.0f;
    }
}
