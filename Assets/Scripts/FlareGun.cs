using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class FlareGun : MonoBehaviour
{
    public InputActionProperty shootAction; // Expects a button.

    public GameObject flare = null;
    public Transform barrelEnd = null;

    public int ammo = 5;
    public int maxAmmo = 10;
    public bool canShoot = true;
    public TextMeshProUGUI ammoText = null;

    private new Animation animation = null;
    // private AudioSource audioSource = null;

    void Start()
    {
        animation = GetComponentInChildren<Animation>();
        // audioSource = GetComponentInChildren<AudioSource>();
        ammoText.text = $"Ammo: {ammo}";
    }

    void Update()
    {
        if (shootAction.action.WasPerformedThisFrame() && CanShoot())
        {
            animation.Play("Shoot");
            Shoot();
        }

        ammoText.text = $"Ammo: {ammo}";
    }

    void Shoot()
    {
        Instantiate(flare, barrelEnd.position, barrelEnd.rotation);
        ammo--;

        // if (audioSource)
        //     audioSource.Play();
    }

    bool CanShoot()
    {
        return ammo > 0 && canShoot;
    }

    public void AddAmmo(int amount)
    {
        ammo = Mathf.Min(ammo + amount, maxAmmo);
    }
}
