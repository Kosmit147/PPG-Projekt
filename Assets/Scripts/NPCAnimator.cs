using UnityEngine;

[RequireComponent(typeof(Animator))]
public class NPCAnimator : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetFloat("MotionSpeed", 1.0f);
    }
}
