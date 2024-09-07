using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerKeyStates
{
    NoKey = 0,
    HasKey = 1,
    ConsumedKey = 2
}

public class GateTotem : MonoBehaviour
{
    [TextArea()]
    [SerializeField] private string noKeyText, hasKeyText;
    [SerializeField] private Sprite newSprite;
    private GateState playerState;
    private DialogueData data;
    private PlayerController player;
    private bool interactable;
    // Start is called before the first frame update
    void Start()
    {
        playerState = GetComponent<GateState>();
        data = GetComponent<DialogueData>();
        player = FindAnyObjectByType<PlayerController>();
    }

    // Update is called once per frame
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
                playerState.ChangeKeyState(player, newSprite, data);
                data.selectedChoice = 0;
                break;
            case 2:
                data.selectedChoice = 0;
                break;
        }
    }

    private void DialogueTrigger()
    {
        if (interactable && Input.GetKeyDown(KeyCode.E) && !data.dialogueActive)
        {
            DialogueOptions();
        }
    }

    private void DialogueOptions()
    {
        if ((PlayerKeyStates)player.keyItemState == PlayerKeyStates.HasKey)
        {
            data.sentences[0] = hasKeyText;
            data.hasChoice = true;
            data.TriggerDialogue();
        }
        else
        {
            data.sentences[0] = noKeyText;
            data.hasChoice = false;
            data.TriggerDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactable = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        interactable = false;
    }
}
