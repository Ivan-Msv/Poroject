using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [field: SerializeField] public int Health { get; private set; }
    [field: SerializeField] public int MoveSpeed { get; private set; }
    [field: SerializeField] public int ProjectileAmount { get; private set; }
    [field: SerializeField] public float ProjectileTriggerRange { get; private set; }
    [field: SerializeField] public float ProjectileFrequency { get; private set; }
    [field: SerializeField] public float ProjectileLifeTime { get; private set; }
    [field: SerializeField] public float ProjectileMoveSpeed { get; private set; }
    [field: SerializeField] public float ProjectileRotationSpeed { get; private set; }

}