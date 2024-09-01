using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEffects/HealthUpgrade")]
public class HealthUpgrade : ItemEffect
{
    [SerializeField] private int amount;
    public override void ApplyEffect(GameObject player)
    {
        player.GetComponent<PlayerHealth>().UpgradeHealth(amount);
    }
}
