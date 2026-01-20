using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(SphereCollider))]
public class NPC : MonoBehaviour
{
    public InputActionProperty talkAction; // Expects a button.
    public TextMeshProUGUI dialogueText = null;
    private bool playerInTalkingRange = false;

    void Update()
    {
        if (playerInTalkingRange && talkAction.action.WasPerformedThisFrame())
            Talk();
    }

    void Talk()
    {
        dialogueText.text = "Hello traveler.";
        CancelInvoke(nameof(ClearDialogueText));
        Invoke(nameof(ClearDialogueText), 2.0f);
    }

    void ClearDialogueText()
    {
        dialogueText.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTalkingRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTalkingRange = false;
        }
    }
}
