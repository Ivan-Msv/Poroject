using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
using Pathfinding;
using UnityEngine.Rendering.UI;

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
    private GameObject player;
    private EnemyHealth health;
    public Vector3 EnemySpawnPoint { get; private set; }
    [field: SerializeField] public EnemyType Type { get; private set; }
    private EnemyStates currentState;
    [SerializeField] private EnemyStats stats;
    [SerializeField] private Vector3[] enemyIdlePoints;
    [SerializeField] private float timeOutOfRange;
    [SerializeField] private float idleMoveCooldown = 1;
    private int idlePoints = 0;
    private bool idleCanMove = true;
    private bool canAggro = true;
    private bool aggro;
    private AIPath enemyPath;
    private Seeker seeker;
    private Path currentPath;
    private AIDestinationSetter destination;
    private int currentWayPoint;
    private float playerDistance;
    private float attackTimer;
    void Start()
    {
        seeker = GetComponent<Seeker>();
        enemyPath = GetComponent<AIPath>();
        destination = GetComponent<AIDestinationSetter>();
        player = FindAnyObjectByType<PlayerHealth>().gameObject;
        health = GetComponent<EnemyHealth>();
        health.SetMaxHealth(stats.Health);
        EnemySpawnPoint = this.transform.position;
        currentState = EnemyStates.Idle;
        enemyPath.maxSpeed = stats.MoveSpeed;
        enemyPath.canMove = false;
        enemyPath.canSearch = false;
        destination.target = player.transform;
        currentPath = seeker.StartPath(transform.position, enemyIdlePoints[idlePoints]);
    }

    void Update()
    {
        playerDistance = Vector2.Distance(player.transform.position, transform.position);
        if (health.currentHealth <= 0)
        {
            Death();
        }
        CurrentEnemyState();
        SwitchStates();
        Debug.LogWarning(seeker.GetCurrentPath().vectorPath.Count);
    }
    private void CurrentEnemyState()
    {
        switch (currentState)
        {
            case EnemyStates.Idle:
                enemyPath.canMove = false;
                IdleMovement();
                break;
            case EnemyStates.FollowingPlayer:
                enemyPath.canMove = true;
                AttackPattern();
                break;
        }
    }
    private void SwitchStates()
    {
        if (currentState == EnemyStates.Idle)
        {
            enemyPath.canSearch = false;
            if (playerDistance <= 6 && canAggro || aggro)
            {
                currentState = EnemyStates.FollowingPlayer;
                aggro = false;
            }
        }
        else if (currentState == EnemyStates.FollowingPlayer)
        {
            enemyPath.canSearch = true;
            enemyPath.autoRepath.mode = AutoRepathPolicy.Mode.Dynamic;
            if (seeker.GetCurrentPath().vectorPath.Count > 7)
            {
                ResetAggro();
                StartCoroutine(AggroCooldown()); // sometimes it glitched between 2 states so this helps with enemy not beign stuck in one place shooting projectiles
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
                if (idleCanMove)
                {
                    transform.position = Vector3.MoveTowards(transform.position, currentPath.vectorPath[currentWayPoint], 1.5f * Time.deltaTime);
                }
            }
        }
    }
    private IEnumerator IdleCooldown()
    {
        idleCanMove = false;
        yield return new WaitForSeconds(idleMoveCooldown);
        idleCanMove = true;
    }
    private IEnumerator AggroCooldown()
    {
        canAggro = false;
        yield return new WaitForSeconds(3);
        canAggro = true;
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
            StartCoroutine(IdleCooldown());
        }
    }
    private void ResetAggro()
    {
        currentWayPoint = 0;
        currentPath = seeker.StartPath(transform.position, enemyIdlePoints[idlePoints]);
        currentState = EnemyStates.Idle;
    }
    private void AttackPattern()
    {
        if (playerDistance <= stats.ProjectileTriggerRange)
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
    }
    private void SuicidalAttack()
    {
        if (attackTimer >= stats.ProjectileFrequency)
        {
            ProjectileManager.instance.SpawnExplodingProjectile(this.transform, stats.ProjectileAmount, stats.ProjectileMoveSpeed, stats.ProjectileLifeTime, Vector3.zero);
            attackTimer = 0f;
        }
    }
    private void GenericAttack()
    {
        if (attackTimer >= stats.ProjectileFrequency)
        {
            Vector3 playerDirection = (ProjectileManager.instance.player.transform.position - transform.position).normalized;
            Quaternion rotationAngle = Quaternion.Euler(0, 0, MathF.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg);
            ProjectileManager.instance.SpawnProjectile(this.transform, 1, stats.ProjectileMoveSpeed, stats.ProjectileLifeTime, playerDirection, rotationAngle);
            attackTimer = 0f;
        }
    }
    private void TankAttack()
    {
        if (attackTimer >= stats.ProjectileFrequency)
        {
            ProjectileManager.instance.SpawnEnemyRotatingProjectiles(this.transform, true, stats.ProjectileAmount, 0, stats.ProjectileLifeTime, stats.ProjectileRotationSpeed, stats.ProjectileMoveSpeed);
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
    public void OnRespawn()
    {
        enemyPath.canMove = false;
        enemyPath.canSearch = false;
        ResetAggro();
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
        }
        if (collision.CompareTag("EnemyTrigger"))
        {
            ResetAggro();
        }
    }
}
