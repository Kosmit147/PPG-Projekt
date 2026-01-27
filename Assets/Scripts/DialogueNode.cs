using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DialogueNode", menuName = "NPC/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    [TextArea(3, 10)]
    public string dialogue;

    public List<Choice> choices;
}

[System.Serializable]
public class Choice
{
    public string buttonText;
    public DialogueNode nextNode;
}
