using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    public string nickname = "";
    [TextArea()]
    public string[] sentences;
    [TextArea()]
    public string choiceSentence;
    public string choice1, choice2;
    public bool hasChoice = false;
    public bool dialogueActive = false;
    public int selectedChoice;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(this);
        dialogueActive = true;
    }

    public void TriggerChoiceDialogue()
    {
        FindObjectOfType<DialogueManager>().StartChoiceDialogue(this);
        dialogueActive = true;
    }
}
