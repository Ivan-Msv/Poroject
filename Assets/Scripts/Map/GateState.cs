using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GateState : MonoBehaviour
{
    [SerializeField] private PlayerKeyStates keyState;
    [TextArea()]
    [SerializeField] private string newDialogue;
    public void ChangeKeyState(PlayerController player, Animator anim, DialogueData data)
    {
        player.keyItemState = (int)keyState;
        anim.Play("Totem Glow");
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponentInChildren<InteractText>().gameObject.SetActive(false);
        data.sentences[0] = newDialogue;
        data.hasChoice = false;
        data.TriggerDialogue();
    }
}