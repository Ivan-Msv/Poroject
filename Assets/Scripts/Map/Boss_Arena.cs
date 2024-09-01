using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Arena : MonoBehaviour
{
    [SerializeField] private Door entranceDoor;
    [SerializeField] private Button shortcutButton;
    [SerializeField] private float speed;
    public bool fightActive;


    public void EnableButton()
    {
        shortcutButton.canBePressed = true;
    }
}
