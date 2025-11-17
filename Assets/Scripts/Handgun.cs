using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class Gun : MonoBehaviour
{
    public InputActionProperty shootAction; // Expects a button.

    public GameObject bullet = null;
    public Transform barrel = null;

    private Animator animator = null;
    private AudioSource audioSource = null;

    [System.Serializable]
    private struct AnimationParamIds
    {
        public int fire;
    }

    private AnimationParamIds animParams = new();

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        RetrieveAnimParamIds();
        audioSource = GetComponentInChildren<AudioSource>();
    }

    void Update()
    {
        if (shootAction.action.WasPerformedThisFrame())
        {
            animator.SetTrigger(animParams.fire);
        }
    }

    void Shoot()
    {
        Instantiate(bullet, barrel.position, barrel.rotation);

        if (audioSource)
            audioSource.Play();
    }

    void CasingRelease() {}

    void RetrieveAnimParamIds()
    {
        animParams = new()
        {
            fire = Animator.StringToHash("Fire"),
        };
    }
}
