using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GateState : MonoBehaviour
{
    [SerializeField] private PlayerKeyStates keyState;
    [SerializeField] private Door gates;
    public void ChangeKeyState(PlayerController player, Animator anim)
    {
        switch (keyState)
        {
            case PlayerKeyStates.ConsumedKey:
                Debug.Log("Yea");
                break;
            case PlayerKeyStates.NoKey:
                player.keyItemState = (int)keyState;
                anim.Play("Totem Glow");
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                gameObject.GetComponentInChildren<InteractText>().gameObject.SetActive(false);
                gates.shouldOpen = true;
                gameObject.GetComponent<SwitchCamera>().SetCamera(4);
                break;
        }
    }
}