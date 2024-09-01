using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueData : MonoBehaviour
{
    public string nickname = "";
    [TextArea()]
    public string[] sentences;
    public bool dialogueActive = false;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(this);
        dialogueActive = true;
    }
}
