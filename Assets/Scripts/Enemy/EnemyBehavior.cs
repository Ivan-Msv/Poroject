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
    public EnemyStats(int health, int moveSpeed, float projectileFrequency, int projectileAmount, float projectileLifeTime)
    {
        this.Health = health;
        this.moveSpeed = moveSpeed;
        this.projectileFrequency = projectileFrequency;
        this.projectileAmount = projectileAmount;
        this.projectileLifeTime = projectileLifeTime;
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GenericAttack();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TankAttack();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {

        }
    }

    private void GenericAttack()
    {
        Vector3 playerDirection = (ProjectileManager.instance.player.transform.position - transform.position).normalized;
        Quaternion rotationAngle = Quaternion.Euler(0, 0, MathF.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg);
        ProjectileManager.instance.SpawnProjectile(this.transform, 1, 5, 1, playerDirection, rotationAngle);
    }
    private void TankAttack()
    {
        ProjectileManager.instance.SpawnRotatingProjectiles(this.transform, stats.projectileAmount, 0, stats.projectileLifeTime, true, false, 0, 0.3f);
        attackTimer = 0f;
        //attackTimer += Time.deltaTime;
        //if (attackTimer >= stats.projectileFrequency)
        //{
        //    ProjectileManager.instance.SpawnRotatingProjectiles(this.transform, stats.projectileAmount, 0, stats.projectileLifeTime, true, false, 0, 0.3f);
        //    attackTimer = 0f;
        //}
    }
    private void DeathCheck()
    {
        if (health.currentHealth <= 0)
        {
            EnemyManager.instance.DisableEnemy(this.gameObject, health);
        }
    }
    private EnemyStats SetStatsBasedOnType()
    {
        EnemyStats stats = new EnemyStats();
        switch (Type)
        {
            case EnemyType.Suicidal:
                stats.Health = 5;
                stats.moveSpeed = 4;
                stats.projectileAmount = 20;
                break;
            case EnemyType.Generic:
                stats.Health = 10;
                stats.moveSpeed = 2;
                stats.projectileFrequency = 0.2f;
                stats.projectileAmount = 1;
                break;
            case EnemyType.Tank:
                stats.Health = 20;
                stats.moveSpeed = 1;
                stats.projectileFrequency = 0.9f;
                stats.projectileAmount = 10;
                stats.projectileLifeTime = 1f;
                break;
        }

        return stats;
    }
}
