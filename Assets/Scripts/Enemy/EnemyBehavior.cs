using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using Pathfinding;


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
    Idle, AwareOfPlayer, FollowingPlayer, MovingBack // 2 other ones became useless but maybe that will change in the future
}
public enum EnemyType
{
    Suicidal, Generic, Tank
}
public class EnemyBehavior : MonoBehaviour
{
    [field: SerializeField] public EnemyType Type { get; private set; }
    [SerializeField] private Vector3[] enemyIdlePoints;
    [SerializeField] private float timeOutOfRange;
    private int idlePoints = 0;
    private GameObject player;
    private EnemyHealth health;
    private AIPath enemyPath;
    private EnemyStats stats;
    private Seeker seeker;
    private Path currentPath;
    private AIDestinationSetter destination;
    private int currentWayPoint;
    private float outOfRangeTimer;
    private bool aggro;
    public Vector3 EnemySpawnPoint { get; private set; }
    [SerializeField] private EnemyStates currentState; // remove serializefield later
    private float attackTimer;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        enemyPath = GetComponent<AIPath>();
        destination = GetComponent<AIDestinationSetter>();
        player = FindAnyObjectByType<PlayerHealth>().gameObject;
        stats = SetStatsBasedOnType();
        health = GetComponent<EnemyHealth>();
        health.SetMaxHealth(stats.Health);
        EnemySpawnPoint = this.transform.position;
        currentState = EnemyStates.Idle;

        destination.target = player.transform;
        currentPath = seeker.StartPath(transform.position, enemyIdlePoints[idlePoints]);
    }

    void Update()
    {
        if (health.currentHealth <= 0)
        {
            Death();
        }
        CurrentEnemyState();
        SwitchStates();
    }
    private void CurrentEnemyState()
    {
        switch (currentState)
        {
            case EnemyStates.Idle:
                enemyPath.enabled = false;
                IdleMovement();
                break;
            case EnemyStates.FollowingPlayer:
                enemyPath.enabled = true;
                AttackPattern();
                break;
        }
    }
    private void SwitchStates()
    {
        float playerDistance = Vector2.Distance(player.transform.position, transform.position);
        if (playerDistance <= 6 || aggro)
        {
            currentState = EnemyStates.FollowingPlayer;
            aggro = false;
        }
        if (playerDistance > 8 && currentState == EnemyStates.FollowingPlayer)
        {
            outOfRangeTimer += Time.deltaTime;
            if (outOfRangeTimer >= timeOutOfRange) // so much nesting...
            {
                currentWayPoint = 0;
                currentPath = seeker.StartPath(transform.position, enemyIdlePoints[idlePoints]);
                currentState = EnemyStates.Idle;
                outOfRangeTimer = 0;
            }
        }
    }
    private void IdleMovement()
    {
        WayPointSystem();

        if (currentWayPoint < currentPath.vectorPath.Count)
        {
            float distance = Vector2.Distance(transform.position, currentPath.vectorPath[currentWayPoint]);
            if (distance <= 0.1f)
            {
                currentWayPoint++;
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, currentPath.vectorPath[currentWayPoint], 1.5f * Time.deltaTime);
            }
        }
    }
    private void WayPointSystem()
    {
        if (currentWayPoint >= currentPath.vectorPath.Count)
        {
            currentWayPoint = 0;
            if (idlePoints == enemyIdlePoints.Length - 1)
            {
                idlePoints = 0;
            }
            else
            {
                idlePoints++;
            }
            currentPath = seeker.StartPath(transform.position, enemyIdlePoints[idlePoints]);
        }
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
    private void Death()
    {
        if (Type == EnemyType.Suicidal)
        {
            ProjectileManager.instance.SpawnRotatingProjectiles(this.transform, 30, 0, 2, true, false, 0, 0.5f);
        }
        EnemyManager.instance.DisableEnemy(this.gameObject, health);
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
                stats.moveSpeed = 3;
                stats.projectileFrequency = 0.6f;
                stats.projectileAmount = 1;
                stats.projectileLifeTime = 1f;
                stats.projectileMoveSpeed = 2f;
                break;
            case EnemyType.Tank:
                stats.Health = 20;
                stats.moveSpeed = 2;
                stats.projectileFrequency = 0.9f;
                stats.projectileAmount = 10;
                stats.projectileLifeTime = 1f;
                stats.projectileMoveSpeed = 0.3f;
                break;
        }

        enemyPath.maxSpeed = stats.moveSpeed;
        enemyPath.enabled = false;

        return stats;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Type == EnemyType.Suicidal && collision.CompareTag("Player"))
        {
            Death();
        }
        if (collision.CompareTag("Player Projectile"))
        {
            aggro = true;
            outOfRangeTimer = 0;
        }
        if (collision.CompareTag("EnemyTrigger"))
        {
            aggro = false;
            outOfRangeTimer = timeOutOfRange;
            currentState = EnemyStates.Idle;
        }
    }
}
