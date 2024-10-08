using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class RespawnPoint : MonoBehaviour
{
    DialogueData data;
    private bool interactable;
    private void Start()
    {
        data = GetComponent<DialogueData>();
    }

    void Update()
    {
        DialogueTrigger();
        ChoiceCheck();
    }

    private void ChoiceCheck()
    {
        switch (data.selectedChoice)
        {
            case 1:
                RespawnManager.instance.SetPlayerRespawn(this.gameObject);
                FindAnyObjectByType<PlayerHealth>().HealToFullHP();
                data.selectedChoice = 0;
                break;
            case 2:
                data.selectedChoice = 0;
                break;
        }
    }

    private void DialogueTrigger()
    {
        if (interactable && Input.GetKeyDown(KeyCode.E) && !DialogueManager.instance.dialogueActive && RespawnManager.instance.Alive)
        {
            DialogueOptions();
        }

    }

    private void DialogueOptions()
    {
        if (RespawnManager.instance.playerRespawnPoint == this.gameObject)
        {
            FindAnyObjectByType<PlayerHealth>().HealToFullHP();
            data.hasChoice = false;
            data.TriggerDialogue();
        }
        else
        {
            data.hasChoice = true;
            data.TriggerChoiceDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactable = false;
        }
    }
}
