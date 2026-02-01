using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;
    public Transform choiceButtonContainer;
    public GameObject choiceButtonPrefab;
    public FpsCamera fpsCamera;
    public FlareGun flareGun;

    public void StartDialogue(DialogueNode startingNode)
    {
        dialogueUI.SetActive(true);
        fpsCamera.enabled = false;
        Cursor.visible = true;
        flareGun.canShoot = false;
        ShowNode(startingNode);
    }

    void ShowNode(DialogueNode node)
    {
        dialogueText.text = node.dialogue;

        foreach (Transform child in choiceButtonContainer)
            Destroy(child.gameObject);

        if (node.choices.Count == 0)
        {
            CreateChoiceButton("End", null);
            return;
        }

        foreach (Choice choice in node.choices)
            CreateChoiceButton(choice.buttonText, choice.nextNode);
    }

    void CreateChoiceButton(string text, DialogueNode nextNode)
    {
        GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceButtonContainer);
        buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = text;

        buttonObj.GetComponent<Button>().onClick.AddListener(() => 
        {
            if (nextNode == null)
                CloseDialogue();
            else
                ShowNode(nextNode);
        });
    }

    void CloseDialogue()
    {
        dialogueUI.SetActive(false);
        fpsCamera.enabled = true;
        Cursor.visible = false;
        flareGun.canShoot = true;
    }
}
