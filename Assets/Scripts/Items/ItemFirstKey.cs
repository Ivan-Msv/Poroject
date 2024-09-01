using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/KeyItem")]
public class ItemFirstKey : ItemEffect
{
    [Tooltip("0: Does not have item, 1: Has item, 2: Has consumed the item")]
    [SerializeField] private int itemState;
    public override void ApplyEffect(GameObject player)
    {
        player.GetComponent<PlayerController>().keyItemState = itemState; // weird to have it in player controller to be honest...
    }
}
