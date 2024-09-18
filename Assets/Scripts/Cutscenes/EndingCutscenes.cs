using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCutscenes : MonoBehaviour
{
    private DialogueData data;
    void Start()
    {
        data = GetComponent<DialogueData>();
    }
    public void FirstEnding()
    {
        data.ending = true;
        data.TriggerDialogue();
    }
}
