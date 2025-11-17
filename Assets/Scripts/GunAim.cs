using UnityEngine;
using UnityEngine.InputSystem;

public class GunAim : MonoBehaviour
{
    public InputActionProperty aimAction; // Expects a button;

    public Transform defaultPosition = null;
    public Transform aimPosition = null;
    public float aimSpeed = 5.0f;

    private float fraction = 0.0f;

    void Update()
    {
        if (aimAction.action.IsPressed())
            fraction = Mathf.Clamp01(fraction + aimSpeed * Time.deltaTime);
        else
            fraction = Mathf.Clamp01(fraction - aimSpeed * Time.deltaTime);

        transform.position = Vector3.Lerp(defaultPosition.position, aimPosition.position, fraction);
    }
}
