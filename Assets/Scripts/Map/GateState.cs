using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GateState : MonoBehaviour
{
    [SerializeField] private PlayerKeyStates keyState;
    [TextArea()]
    [SerializeField] private string newDialogue;
    public void ChangeKeyState(PlayerController player, Sprite replaceSprite, DialogueData data)
    {
        player.keyItemState = (int)keyState;
        gameObject.GetComponent<SpriteRenderer>().sprite = replaceSprite;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponentInChildren<InteractText>().gameObject.SetActive(false);
        data.sentences[0] = newDialogue;
        data.hasChoice = false;
        data.TriggerDialogue();
    }
}