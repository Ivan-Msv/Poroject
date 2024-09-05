using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GateState : MonoBehaviour
{
    [SerializeField] private PlayerKeyStates keyState;
    public void ChangeKeyState(PlayerController player)
    {
        player.keyItemState = (int)keyState;
    }
}