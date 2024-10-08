using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class FirstBoss : MonoBehaviour
{
    [Header("Main Settings")]
    [SerializeField] private string bossName;
    [SerializeField] private Canvas bossUI;
    [SerializeField] private GameObject player;
    [SerializeField] private Boss_Arena bossArea;
    [SerializeField] private GameObject keyItem;
    [Space]
    [Header("Projectile List")]
    [SerializeField] private GameObject axisProjectile;
    [SerializeField] private GameObject explodingProjectile;
    [Space]
    [Header("Rotating Projectile Settings")]
    [SerializeField] private int rotatingProjectileAmount;
    [SerializeField] private int rotatingProjectileRadius;
    private int axisProjectileAmount;
    private float axisProjectileRadius;
    private float axisProjectileSpeed;
    private float axisProjectileSpeedMultiplier;
    [Space]
    [Header("Attack Settings")]
    private float attackTimer;
    private float attackTimerDuration;
    [SerializeField] private float attackCooldownDuration = 1.5f;
    [Space]
    [Header("Pattern 1 settings")]
    [SerializeField] private float p1ProjectileFrequency = 0.6f;
    [SerializeField] private float p1AttackDuration = 5;
    [SerializeField] private Vector3 p1MiddlePosition;
    public float p1Angle;
    [Space]
    [Header("Pattern 2 settings")]
    [SerializeField] private float p2ProjectileFrequency = 0.3f;
    [SerializeField] private int p2ProjectileAmount = 25;
    [SerializeField] private float p2ProjectileRadius = 40;
    [SerializeField] private float p2ProjectileSpeed = 1;
    [SerializeField] private float p2ProjectileSpeedMultiplier = 6;
    [Space]
    [SerializeField] private float p2AttackDuration = 7.2f;
    [SerializeField] private float p2MoveRadiusX = 5;
    [SerializeField] private float p2MoveSpeed = 20;
    private Vector3 leftBound;
    private Vector3 rightBound;
    [Space]
    [Header("Pattern 3 settings")]
    [SerializeField] private float p3ProjectileFrequency;
    [SerializeField] private int p3ProjectileAmount = 1;
    [SerializeField] private float p3ProjectileRadius = 0;
    [SerializeField] private float p3ProjectileSpeed = 3;
    [SerializeField] private float p3ProjectileSpeedMultiplier = 2;
    [Space]
    [SerializeField] private float p3AttackDuration = 5;
    [SerializeField] private float p3RotationSpeed = 3;
    [SerializeField] private float p3RotationDistance = 5;
    private float p3CurrentRotationSpeed;
    private float p3Angle = 0;
    [Space]
    [Header("Survival pattern settings")]
    [SerializeField] private float spProjectileFrequency;
    [SerializeField] private float spProjectileRotationSpeed;
    [SerializeField] private float spAttackDuration = 10;
    private float spAngle;
    [Space]
    [Header("Idle")]
    [SerializeField] private float idleAngle = 0;
    [SerializeField] private Vector3 idlePositionStart;

    private int moveStage = 0;
    private bool canAttack = true;
    public bool insideWall = false;
    private int lastAttack;
    private int attackChoice = 0;
    private float projectileTimer;
    private EnemyHealth healthSystem;
    private bool fightActive;
    public bool altAngle;

    void Start()
    {
        idlePositionStart = transform.position;
        p3CurrentRotationSpeed = p3RotationSpeed;
        healthSystem = GetComponent<EnemyHealth>();
        bossUI.GetComponentInChildren<TextMeshProUGUI>().text = bossName;
    }

    void Update()
    {
        fightActive = bossArea.fightActive;

        if (attackChoice != 0)
        {
            attackTimer += Time.deltaTime;
        }

        if (fightActive && RespawnManager.instance.Alive)
        {
            bossUI.enabled = true;
            Attack();
            AttackWheel();
        }
        else
        {
            bossUI.enabled = false;
            healthSystem.HealToFull();
            PreBossFightMovement();
        }
    }
    private void PreBossFightMovement()
    {
        if (transform.rotation != Quaternion.Euler(0, 0, 0))
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), 2);
        }
        Vector3 spawnPosition = new Vector3(39.77f, 33.71f, 0);
        if ((transform.position - spawnPosition).magnitude > 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, spawnPosition, 3 * Time.deltaTime); // so he doesn't just teleport on player death
        }
        else
        {
            float xPos = spawnPosition.x + MathF.Cos(idleAngle) * 0.1f;
            float yPos = spawnPosition.y + MathF.Sin(idleAngle) * 0.1f;

            idleAngle += 5 * Time.deltaTime;
            transform.position = new Vector3(xPos, yPos, 0);
        }
    }
    public void SpawnProjectiles(GameObject projectile, int amount, float radius, Vector3 axis, Quaternion? rotation = null)
    {
        Quaternion newRotation = rotation ?? projectile.transform.rotation;

        Vector3 leftBound = transform.position - new Vector3(radius, 0, 0);
        Vector3 rightBound = transform.position + new Vector3(radius, 0, 0);
        float stepX;

        if (amount >= 1)
        {
            stepX = (rightBound.x - leftBound.x) / amount;
        }
        else
        {
            stepX = (rightBound.x - leftBound.x) / (amount - 1);
        }

        for (int i = 0; i < amount; i++)
        {
            float xPos = leftBound.x + stepX * i;
            Vector3 newPos = new Vector3(xPos, transform.position.y, 0);
            GameObject newProjectile = ProjectilePoolSystem.instance.GetObject(projectile, newPos, newRotation);
            if (projectile.GetComponent<AxisProjectile>())
            {
                newProjectile.GetComponent<AxisProjectile>().SetDirection(transform.position, axis, axisProjectileSpeed, axisProjectileSpeedMultiplier, attackTimerDuration);
            }
            else if (projectile.GetComponent<ExplodingProjectile>())
            {
                newProjectile.GetComponent<ExplodingProjectile>().SetDirection(transform.position, axis, 1, attackTimerDuration);
            }
            else
            {
                Debug.LogWarning("Cannot get component from set projectile.");
            }
        }
    }
    private IEnumerator AttackCooldownTimer()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownDuration);
        canAttack = true;
        attackTimer = 0f;
    }
    private void Attack()
    {
        if (canAttack && attackTimer <= attackTimerDuration)
        {
            AttackCancelCheck();

            projectileTimer += Time.deltaTime;
            if (attackChoice == 1)
            {
                FirstAttackPattern();
            }

            else if (attackChoice == 2)
            {
                SecondAttackPattern();
            }

            else if (attackChoice == 3)
            {
                ThirdAttackPattern();
            }

            else if (attackChoice == 5)
            {
                StartCoroutine(SurvivalPhase());
            }
        }
        else
        {
            Idle();
            BeginCooldown();
        }
    }
    private void AttackCancelCheck()
    {
        if (healthSystem.currentHealth <= 0 && attackChoice != 5 && attackChoice != 0)
        {
            ProjectileManager.instance.DisableAllProjectiles();
            CameraShake.instance.ShakeCamera(7, 0.2f);
            AudioManager.instance.PlaySound("BossSurvivalPhase");
            attackTimer = 200;
            BeginCooldown();
        }
    }
    private void BeginCooldown()
    {
        if (attackTimer >= attackTimerDuration && canAttack)
        {
            StartCoroutine(AttackCooldownTimer());
            lastAttack = attackChoice;
            attackChoice = 0;
            moveStage = 0;
            p3CurrentRotationSpeed = p3RotationSpeed;
            p1Angle = 0;
            idlePositionStart = transform.position;
            idleAngle = Mathf.Atan2(idlePositionStart.y, idlePositionStart.x);
        }
    }
    private void Idle()
    {
        if (!canAttack)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), 2);
            if (insideWall)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 6 * Time.deltaTime);
                idlePositionStart = transform.position;
            }
            else
            {
                float xPos = idlePositionStart.x + MathF.Cos(idleAngle) * 0.1f;
                float yPos = idlePositionStart.y + MathF.Sin(idleAngle) * 0.1f;

                idleAngle += 5 * Time.deltaTime;
                transform.position = new Vector3(xPos, yPos, 0);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            insideWall = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            insideWall = false;
        }
    }
    private void AttackCircle()
    {
        int reduceObjects = 0;
        for (int i = 5; i < 10; i++)
        {
            ProjectileManager.instance.SpawnRotatingProjectiles(player.transform, 50 - reduceObjects,  i, attackTimerDuration, false);
            reduceObjects += 10;
        }
        p1MiddlePosition = player.transform.position;
    }
    private void SurvivalAttackCircle()
    {
        int reduceObjects = 0;
        for (int i = 15; i < 20; i++)
        {
            ProjectileManager.instance.SpawnRotatingProjectiles(player.transform, 100 + reduceObjects, i, attackTimerDuration, false, 5);
            reduceObjects -= i;
        }
    }
    private void FirstAttackPattern()
    {
        FirstAttackMovement();
        if (projectileTimer >= p1ProjectileFrequency)
        {
            AudioManager.instance.PlaySound("rotatingprojectile");
            if (attackTimer <= attackTimerDuration / 2)
            {
                ProjectileManager.instance.SpawnRotatingProjectiles(this.transform, rotatingProjectileAmount, 0, 3f, true, 40, 2, altAngle);
                p1Angle += 30;
            }
            else
            {
                ProjectileManager.instance.SpawnRotatingProjectiles(this.transform, rotatingProjectileAmount, 0, 3f, true, -40, 2, altAngle);
                p1Angle -= 30;
            }
            altAngle = !altAngle;
            projectileTimer = 0f;
        }
    }

    private void FirstAttackMovement()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, p1Angle), 1);
        if (transform.position != p1MiddlePosition)
        {
            projectileTimer = 0;

            if (Vector3.Distance(transform.position, p1MiddlePosition) > 3)
            {
                transform.position = Vector3.MoveTowards(transform.position, p1MiddlePosition, 8 * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, p1MiddlePosition, 3 * Time.deltaTime);
            }
        }
    }
    private void SecondAttackPattern()
    {
        SecondAttackMovement();
        if (projectileTimer >= p2ProjectileFrequency && moveStage != 0)
        {
            switch(moveStage)
            {
                case 1:
                    SpawnProjectiles(axisProjectile, axisProjectileAmount, axisProjectileRadius, new Vector3(0.65f, -1, 0), Quaternion.Euler(0, 0, -65));
                    break;
                case 2:
                    SpawnProjectiles(axisProjectile, axisProjectileAmount, axisProjectileRadius, new Vector3(-0.65f, -1, 0), Quaternion.Euler(0, 0, 65));
                    break;
            }
            AudioManager.instance.PlaySound("axisprojectile");
            projectileTimer = 0f;
        }
    }
    private void SecondAttackMovement()
    {
        Vector3 startPos = player.transform.position + new Vector3(0, 4, 0);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 90), 2);
        if (moveStage != 0)
        {
            switch (moveStage)
            {
                case 1:
                    transform.position = Vector3.MoveTowards(transform.position, rightBound, p2MoveSpeed * Time.deltaTime);
                    if (transform.position.x >= rightBound.x)
                    {
                        moveStage = 2;
                    }
                    break;
                case 2:
                    transform.position = Vector3.MoveTowards(transform.position, leftBound, p2MoveSpeed * Time.deltaTime);
                    if (transform.position.x <= leftBound.x)
                    {
                        moveStage = 1;
                    }
                    break;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, 20 * Time.deltaTime);
            if (transform.position == startPos)
            {
                moveStage = 1;
                SpawnProjectiles(axisProjectile, 60, 45, Vector3.zero, Quaternion.Euler(0, 0, 0));
                leftBound = transform.position - new Vector3(p2MoveRadiusX, 0, 0);
                rightBound = transform.position + new Vector3(p2MoveRadiusX, 0, 0);
            }
        }
    }
    private void ThirdAttackPattern()
    {
        ThirdAttackMovement();
        if (projectileTimer >= p3ProjectileFrequency && moveStage != 0)
        {
            Vector3 moveDirection = (player.transform.position - transform.position).normalized;
            Quaternion rotationAngle = Quaternion.Euler(0, 0, MathF.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg);
            SpawnProjectiles(axisProjectile, p3ProjectileAmount, p3ProjectileRadius, moveDirection, rotationAngle);
            AudioManager.instance.PlaySound("axisprojectile");
            projectileTimer = 0;
        }
    }
    private void ThirdAttackMovement()
    {
        float xPos = player.transform.position.x + MathF.Cos(p3Angle) * p3RotationDistance;
        float yPos = player.transform.position.y + MathF.Sin(p3Angle) * p3RotationDistance;
        Vector3 moveDirection = (transform.position - new Vector3(xPos, yPos, 0)).normalized;
        Vector3 rotationAngle = new Vector3(0, 0, MathF.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg);
        Vector3 newpos = new Vector3(xPos, yPos, 0);

        if (moveStage != 0)
        {
            transform.SetPositionAndRotation(newpos, Quaternion.Euler(rotationAngle));
            p3Angle += p3CurrentRotationSpeed * Time.deltaTime;
            p3CurrentRotationSpeed -= 0.3f * Time.deltaTime;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, newpos, 20 * Time.deltaTime);
            if ((transform.position - newpos).magnitude < 0.01f)
            {
                moveStage = 1;
            }
        }
    }
    private IEnumerator SurvivalPhase()
    {
        transform.rotation *= Quaternion.Euler(0, 0, 1);

        float xPos = MathF.Cos(spAngle);
        float yPos = MathF.Sin(spAngle);
        Vector3 verticalDirection = new Vector3(xPos, yPos, 0).normalized;
        Vector3 horizontalDirection = new Vector3(-yPos, xPos, 0).normalized;
        if (projectileTimer >= spProjectileFrequency && attackTimer <= attackTimerDuration - 1)
        {
            AudioManager.instance.PlaySound("ExplodingMain");
            AudioManager.instance.PlaySound("explodingprojectile", 1);
            SpawnProjectiles(explodingProjectile, 1, 0, verticalDirection);
            SpawnProjectiles(explodingProjectile, 1, 0, -verticalDirection);
            SpawnProjectiles(explodingProjectile, 1, 0, horizontalDirection);
            SpawnProjectiles(explodingProjectile, 1, 0, -horizontalDirection);
            projectileTimer = 0;
        }
        spAngle += spProjectileRotationSpeed * Time.deltaTime;

        yield return new WaitForSeconds(attackTimerDuration);
        Death();
    }
    private void Death()
    {
        if (RespawnManager.instance.Alive && fightActive)
        {
            AudioManager.instance.PlaySound("bossdeath");
            ProjectileManager.instance.SpawnRotatingProjectiles(this.transform, 50, 0, 5, true, 0, 0.3f);
            Instantiate(keyItem, transform.position, keyItem.transform.rotation);
            bossArea.EnableButton();
            gameObject.SetActive(false);
        }
    }
    private void AttackWheel(int choice = -1)
    {
        if (choice == -1)
        {
            choice = AttackChoiceCalc();
        }

        if (canAttack && attackChoice == 0)
        {
            switch (choice)
            {
                case 1:
                    attackTimerDuration = p1AttackDuration;
                    AttackCircle();
                    attackChoice = 1;
                    break;
                case 2:
                    attackTimerDuration = p2AttackDuration;
                    axisProjectileSpeed = p2ProjectileSpeed;
                    axisProjectileAmount = p2ProjectileAmount;
                    axisProjectileRadius = p2ProjectileRadius;
                    axisProjectileSpeedMultiplier = p2ProjectileSpeedMultiplier;
                    attackChoice = 2;
                    break;
                case 3:
                    attackTimerDuration = p3AttackDuration;
                    axisProjectileSpeed = p3ProjectileSpeed;
                    axisProjectileAmount = p3ProjectileAmount;
                    axisProjectileRadius = p3ProjectileRadius;
                    axisProjectileSpeedMultiplier = p3ProjectileSpeedMultiplier;
                    attackChoice = 3;
                    break;
                case 5:
                    attackTimerDuration = spAttackDuration;
                    SurvivalAttackCircle();
                    attackChoice = 5;
                    break;
                default:
                    Debug.Log($"Error, attackpatternchoice: {choice}");
                    break;
            }
        }
    }
    private int FirstAttackChance()
    {
        int attackChance;
        if (lastAttack != 1)
        {
            attackChance = 4;
        }
        else
        {
            attackChance = 2;
        }

        return attackChance;
    }
    private int SecondAttackChance()
    {
        int attackChance;
        if (lastAttack != 2)
        {
            attackChance = 4;
        }
        else
        {
            attackChance = 2;
        }

        return attackChance;
    }
    private int ThirdAttackChance()
    {
        int attackChance;
        if (lastAttack != 3)
        {
            attackChance = 4;
        }
        else
        {
            attackChance = 2;
        }

        return attackChance;
    }
    private int AttackChoiceCalc()
    {
        int finalNumber = 0;
        int attackOne = FirstAttackChance();
        int attackTwo = SecondAttackChance();
        int attackThree = ThirdAttackChance();

        IEnumerable<int> attackOneRange = Enumerable.Range(1, attackOne);
        IEnumerable<int> attackTwoRange = Enumerable.Range(attackOne, attackTwo);
        IEnumerable<int> attackThreeRange = Enumerable.Range(attackOne + attackTwo, attackThree);

        int sumOfAll = attackOne + attackTwo + attackThree;

        int randomNumber = Random.Range(1, sumOfAll);
        bool survivalPhase = healthSystem.currentHealth <= 0;

        switch (survivalPhase && attackChoice == 0)
        {
            case false:
                if (attackOneRange.Contains(randomNumber))
                {
                    finalNumber = 1;
                }
                else if (attackTwoRange.Contains(randomNumber))
                {
                    finalNumber = 2;
                }
                else if (attackThreeRange.Contains(randomNumber))
                {
                    finalNumber = 3;
                }
                else
                {
                    Debug.Log($"Attackhoice error: RNGNum: {randomNumber}");
                }
                break;
            case true:
                finalNumber = 5;
                break;
        }

        return finalNumber;
    }
}
