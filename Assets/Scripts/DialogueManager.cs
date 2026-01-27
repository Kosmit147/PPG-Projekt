using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public Transform choiceButtonContainer;
    public GameObject choiceButtonPrefab;

    public void StartDialogue(DialogueNode startingNode)
    {
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
        dialogueText.transform.parent.gameObject.SetActive(false);
    }
}
