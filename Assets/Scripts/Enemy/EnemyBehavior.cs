using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;


public struct EnemyStats
{
    public int Health;
    public int moveSpeed;
    public float projectileFrequency;
    public int projectileAmount;
    public float projectileLifeTime;
    public float projectileMoveSpeed;
    public EnemyStats(int health, int moveSpeed, float projectileFrequency, int projectileAmount, float projectileLifeTime, float projectileMoveSpeed)
    {
        this.Health = health;
        this.moveSpeed = moveSpeed;
        this.projectileFrequency = projectileFrequency;
        this.projectileAmount = projectileAmount;
        this.projectileLifeTime = projectileLifeTime;
        this.projectileMoveSpeed = projectileMoveSpeed;
    }
}
enum EnemyStates
{
    Idle, AwareOfPlayer, FollowingPlayer, MovingBack
}
public enum EnemyType
{
    Suicidal, Generic, Tank
}
public class EnemyBehavior : MonoBehaviour
{
    [field: SerializeField] public EnemyType Type { get; private set; }
    private EnemyHealth health;
    private EnemyStats stats;
    public Vector3 EnemySpawnPoint { get; private set; }
    private EnemyStates currentState;
    private float attackTimer;
    void Start()
    {
        stats = SetStatsBasedOnType();
        health = GetComponent<EnemyHealth>();
        health.SetMaxHealth(stats.Health);
        EnemySpawnPoint = this.transform.position;
    }

    void Update()
    {
        DeathCheck();
        AttackPattern();
    }

    private void AttackPattern()
    {
        attackTimer += Time.deltaTime;
        switch (Type)
        {
            case EnemyType.Suicidal:
                SuicidalAttack();
                break;
            case EnemyType.Generic:
                GenericAttack();
                break;
            case EnemyType.Tank:
                TankAttack();
                break;
        }
    }


    private void SuicidalAttack()
    {
        if (attackTimer >= stats.projectileFrequency)
        {
            ProjectileManager.instance.SpawnExplodingProjectile(this.transform, stats.projectileAmount, stats.projectileMoveSpeed, stats.projectileLifeTime, Vector3.zero);
            attackTimer = 0f;
        }
    }
    private void GenericAttack()
    {
        if (attackTimer >= stats.projectileFrequency)
        {
            Vector3 playerDirection = (ProjectileManager.instance.player.transform.position - transform.position).normalized;
            Quaternion rotationAngle = Quaternion.Euler(0, 0, MathF.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg);
            ProjectileManager.instance.SpawnProjectile(this.transform, 1, stats.projectileMoveSpeed, stats.projectileLifeTime, playerDirection, rotationAngle);
            attackTimer = 0f;
        }
    }
    private void TankAttack()
    {
        if (attackTimer >= stats.projectileFrequency)
        {
            ProjectileManager.instance.SpawnRotatingProjectiles(this.transform, stats.projectileAmount, 0, stats.projectileLifeTime, true, false, 0, stats.projectileMoveSpeed);
            attackTimer = 0f;
        }
    }
    private void DeathCheck()
    {
        if (health.currentHealth <= 0)
        {
            if (Type == EnemyType.Suicidal)
            {
                ProjectileManager.instance.SpawnRotatingProjectiles(this.transform, 30, 0, 2, true, false, 0, 0.5f);
            }
            EnemyManager.instance.DisableEnemy(this.gameObject, health);
        }
    }
    private EnemyStats SetStatsBasedOnType()
    {
        EnemyStats stats = new();
        switch (Type)
        {
            case EnemyType.Suicidal:
                stats.Health = 5;
                stats.moveSpeed = 4;
                stats.projectileFrequency = 2f;
                stats.projectileAmount = 1;
                stats.projectileLifeTime = 1;
                stats.projectileMoveSpeed = 0;
                break;
            case EnemyType.Generic:
                stats.Health = 10;
                stats.moveSpeed = 2;
                stats.projectileFrequency = 0.6f;
                stats.projectileAmount = 1;
                stats.projectileLifeTime = 1f;
                stats.projectileMoveSpeed = 2f;
                break;
            case EnemyType.Tank:
                stats.Health = 20;
                stats.moveSpeed = 1;
                stats.projectileFrequency = 0.9f;
                stats.projectileAmount = 10;
                stats.projectileLifeTime = 1f;
                stats.projectileMoveSpeed = 0.3f;
                break;
        }

        return stats;
    }
}
