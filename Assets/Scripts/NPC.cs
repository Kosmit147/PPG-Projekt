using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(SphereCollider))]
public class NPC : MonoBehaviour
{
    public InputActionProperty talkAction; // Expects a button.
    private bool playerInTalkingRange = false;
    public DialogueManager dialogueManager;
    public DialogueNode dialogueStart;

    void Update()
    {
        if (playerInTalkingRange && talkAction.action.WasPerformedThisFrame())
            Talk();
    }

    void Talk()
    {
        dialogueManager.StartDialogue(dialogueStart);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTalkingRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerInTalkingRange = false;
    }
}
