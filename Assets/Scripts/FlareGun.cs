using UnityEngine;
using UnityEngine.InputSystem;

public class FlareGun : MonoBehaviour
{
    public InputActionProperty shootAction; // Expects a button.

    public GameObject flare = null;
    public Transform barrelEnd = null;

    private new Animation animation = null;
    // private AudioSource audioSource = null;

    void Start()
    {
        animation = GetComponentInChildren<Animation>();
        // audioSource = GetComponentInChildren<AudioSource>();
    }

    void Update()
    {
        if (shootAction.action.WasPerformedThisFrame())
        {
            animation.Play("Shoot");
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(flare, barrelEnd.position, barrelEnd.rotation);

        // if (audioSource)
        //     audioSource.Play();
    }
}
