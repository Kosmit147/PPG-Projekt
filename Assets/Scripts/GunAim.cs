using UnityEngine;
using UnityEngine.InputSystem;

public class GunAim : MonoBehaviour
{
    public InputActionProperty aimAction; // Expects a button;

    public Transform defaultPosition = null;
    public Transform aimPosition = null;
    public float aimSpeed = 15.0f;

    public Player player = null;
    public Camera fpsCamera = null;
    public float defaultFov = 60.0f;
    public float aimFov = 70.0f;
    public float defaultMoveSpeed = 2.0f;
    public float aimMoveSpeed = 1.0f;

    private float fraction = 0.0f;

    void Update()
    {
        if (aimAction.action.IsPressed())
            fraction = Mathf.Clamp01(fraction + aimSpeed * Time.deltaTime);
        else
            fraction = Mathf.Clamp01(fraction - aimSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(defaultPosition.position, aimPosition.position, fraction);
        fpsCamera.fieldOfView = Mathf.Lerp(defaultFov, aimFov, fraction);
        player.moveSpeed = Mathf.Lerp(defaultMoveSpeed, aimMoveSpeed, fraction);
    }
}
