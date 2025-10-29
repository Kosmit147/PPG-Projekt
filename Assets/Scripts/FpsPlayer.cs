using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FpsPlayer : MonoBehaviour
{
    [Header("Input")]
    public InputActionProperty lookAction;
    public float lookSpeed = 0.1f;

    private Vector2 eulerAngles = Vector2.zero;

    void Start()
    {
        eulerAngles = transform.eulerAngles;
    }

    void Update()
    {
        var look = lookSpeed * lookAction.action.ReadValue<Vector2>();

        eulerAngles.x += look.y;
        eulerAngles.y += look.x;

        var rotationX = Quaternion.AngleAxis(eulerAngles.x, Vector3.left);
        var rotationY = Quaternion.AngleAxis(eulerAngles.y, Vector3.up);

        transform.rotation = (rotationY * rotationX).normalized;
    }
}
