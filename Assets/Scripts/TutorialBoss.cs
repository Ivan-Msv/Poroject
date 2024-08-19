using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TMPro;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class TutorialBoss : MonoBehaviour
{
    [Header("Player Related")]
    [SerializeField] private GameObject player;
    [SerializeField] private float distanceFromPlayer;
    [Space]
    [Header("Projectile List")]
    [SerializeField] private GameObject projectileTriangle;
    [SerializeField] private GameObject projectileDiamond;
    [Space]
    [Header("Rotating Projectile Settings")]
    [SerializeField] private int rotatingProjectileAmount;
    [SerializeField] private int rotatingProjectileRadius;
    [Space]
    [Header("Axis Projectile Settings")]
    [SerializeField] private int axisProjectileAmount;
    [SerializeField] private int axisProjectileRadius;
    [Space]
    [Header("Attack Settings")]
    [SerializeField] private float attackTimerStart;
    [SerializeField] private float attackTimerDuration;
    [SerializeField] private float attackCooldownDuration = 1.5f;
    [SerializeField] private int moveXRadius;
    [SerializeField] private float moveXSpeed;
    [SerializeField] private int moveStage = 0;
    [Space]
    [Header("Pattern 1 settings")]
    [SerializeField] private float p1ProjectileDuration = 0.6f;
    [Space]
    [Header("Pattern 2 settings")]
    [SerializeField] private float p2ProjectileDuration = 0.3f;
    [Space]
    [Header("Pattern 3 settings")]
    [SerializeField] private float p3ProjectileDuration;
    [Space]
    [Header("Pattern 4 settings")]
    [SerializeField] private float p4ProjectileDuration;
    [Space]
    [Header("Idle")]
    [SerializeField] private float idleAngle = 0;
    [SerializeField] private Vector3 idlePositionStart;
   
    private Vector3 leftBound;
    private Vector3 rightBound;

    private bool canAttack = true;
    private int lastAttack;
    private int attackChoice;
    private float projectileTimer;
    void Start()
    {

    }

    void Update()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (attackChoice != 0)
        {
            attackTimerStart += Time.deltaTime;
        }
        Attack();
        //AttackWheel();

        bool debug = true;
        //debug
        if (debug && attackChoice == 0)
        {
            canAttack = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && debug)
        {
            AttackCircle();
            attackChoice = 1;
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && debug)
        {
            attackChoice = 2;
            canAttack = true;
        }
    }
    private void SpawnRotatingProjectiles(GameObject projectile, int amount, int radius, bool shouldMoveForward, bool aroundThePlayer)
    {
        float angleStep = 360f / amount;
        float angle = 0f;
        for (int i = 0; i < amount; i++)
        {
            Vector3 currentPos = transform.position;
            switch (aroundThePlayer)
            {
                case true:
                    currentPos = player.transform.position;
                    break;
                case false:
                    currentPos = transform.position;
                    break;
            }

            if (radius != 0)
            {
                float projectileXDirection = currentPos.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
                float projectileYDirection = currentPos.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

                Vector3 projectileVector = new Vector3(projectileXDirection, projectileYDirection, 0);
                Vector3 projectileMoveDirection = (projectileVector - transform.position).normalized;

                GameObject projectileInstance = Instantiate(projectile, projectileVector, transform.rotation);
                projectileInstance.GetComponent<RotatingProjectile>().SetDirection(projectileMoveDirection, currentPos, shouldMoveForward);
            }
            else
            {
                float projectileXDirection = currentPos.x + Mathf.Sin((angle * Mathf.PI) / 180);
                float projectileYDirection = currentPos.y + Mathf.Cos((angle * Mathf.PI) / 180);

                Vector3 projectileVector = new Vector3(projectileXDirection, projectileYDirection, 0);
                Vector3 projectileMoveDirection = (projectileVector - transform.position).normalized;

                GameObject projectileInstance = Instantiate(projectile, currentPos, transform.rotation);
                projectileInstance.GetComponent<RotatingProjectile>().SetDirection(projectileMoveDirection, currentPos, shouldMoveForward);
            }

            angle += angleStep;
        }
    }
    private void SpawnProjectiles(GameObject projectile, int amount, int radius, Vector3 axis, Quaternion? rotation = null)
    {
        Quaternion newRotation = rotation ?? projectile.transform.rotation;

        Vector3 leftBound = transform.position - new Vector3(radius, 0, 0);
        Vector3 rightBound = transform.position + new Vector3(radius, 0, 0);


        float stepX = (rightBound.x - leftBound.x) / (amount - 1);

        for (int i = 0; i < amount; i++)
        {
            float xPos = leftBound.x + stepX * i;
            Vector3 newPos = new Vector3(xPos, transform.position.y, 0);
            GameObject newProjectile = Instantiate(projectile, newPos, newRotation);
            newProjectile.GetComponent<AxisProjectile>().SetDirection(transform.position, axis);
        }
    }

    private IEnumerator AttackCooldownTimer()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCooldownDuration);
        canAttack = true;
        attackTimerStart = 0f;
    }
    private void Attack()
    {
        if (canAttack && attackTimerStart < attackTimerDuration)
        {
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
                Debug.Log("Attack 3"); // Make it dash towards you while shooting  EXPLODING projectiles upwards and downwards
            }

            else if (attackChoice == 4)
            {
                Debug.Log("Attack 4"); // Something maybe even scrap 4th attack
            }
        }
        else
        {
            Idle();
        }

        BeginCooldown();
    }

    private void BeginCooldown()
    {
        if (attackTimerStart >= attackTimerDuration && canAttack)
        {
            if (lastAttack == 2)
            {
                
            }

            StartCoroutine(AttackCooldownTimer());
            lastAttack = attackChoice;
            attackChoice = 0;
            moveStage = 0;

            idlePositionStart = transform.position;
            idleAngle = Mathf.Atan2(idlePositionStart.y, idlePositionStart.x);
        }
    }
    private void Idle()
    {
        float xPos = idlePositionStart.x + MathF.Cos(idleAngle) * 0.1f;
        float yPos = idlePositionStart.y + MathF.Sin(idleAngle) * 0.1f;

        idleAngle += 5 * Time.deltaTime;
        transform.position = new Vector3(xPos, yPos, 0);
    }
    private void AttackCircle()
    {
        int reduceofObjects = 0;
        for (int i = 5; i < 10; i++)
        {
            SpawnRotatingProjectiles(projectileTriangle, 50 - reduceofObjects, i, false, true);
            reduceofObjects += 10;
        }
    }
    private void FirstAttackPattern()
    {
        if (distanceFromPlayer > 5)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 6 * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 2 * Time.deltaTime);
            if (projectileTimer >= p1ProjectileDuration)
            {
                SpawnRotatingProjectiles(projectileTriangle, rotatingProjectileAmount, 0, true, false);
                projectileTimer = 0f;
            }
        }
    }
    private void SecondAttackPattern()
    {
        SecondAttackMovement();
        if (projectileTimer >= p2ProjectileDuration && moveStage != 0)
        {
            switch(moveStage)
            {
                case 1:
                    SpawnProjectiles(projectileDiamond, axisProjectileAmount, axisProjectileRadius, new Vector3(0.65f, -1, 0), Quaternion.Euler(0, 0, -65));
                    break;
                case 2:
                    SpawnProjectiles(projectileDiamond, axisProjectileAmount, axisProjectileRadius, new Vector3(-0.65f, -1, 0), Quaternion.Euler(0, 0, 65));
                    break;
            }
            projectileTimer = 0f;
        }
    }
    private void SecondAttackMovement()
    {
        Vector3 startPos = player.transform.position + new Vector3(0, 4, 0);

        if (moveStage != 0)
        {
            switch (moveStage)
            {
                case 1:
                    transform.position = Vector3.MoveTowards(transform.position, rightBound, moveXSpeed * Time.deltaTime);
                    if (transform.position.x >= rightBound.x)
                    {
                        moveStage = 2;
                    }
                    break;
                case 2:
                    transform.position = Vector3.MoveTowards(transform.position, leftBound, moveXSpeed * Time.deltaTime);
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
                leftBound = transform.position - new Vector3(moveXRadius, 0, 0);
                rightBound = transform.position + new Vector3(moveXRadius, 0, 0);
            }
        }
    }
    private void AttackWheel()
    {
        int attackPatternChoice = AttackChoiceCalc();
        if (canAttack && attackChoice == 0)
        {
            switch (attackPatternChoice)
            {
                case 1:
                    AttackCircle();
                    attackChoice = 1;
                    break;
                case 2:
                    attackChoice = 2;
                    break;
                case 3:
                    attackChoice = 3;
                    break;
                case 4: 
                    attackChoice = 4;
                    break;
                default:
                    Debug.Log($"Error, attackpatternchoice: {attackPatternChoice}");
                    break;
            }
        }
    }

    private int FirstAttackChance()
    {
        int attackChance;
        if (distanceFromPlayer < 4.2f && lastAttack != 1)
        {
            attackChance = 8;
        }
        else
        {
            attackChance = 0;
        }

        return attackChance;
    }
    private int SecondAttackChance()
    {
        int attackChance;
        if (distanceFromPlayer > 5 && lastAttack != 2)
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
        if (distanceFromPlayer > 5 && lastAttack != 3)
        {
            attackChance = 4;
        }
        else
        {
            attackChance = 2;
        }

        return attackChance;
    }
    private int FourthAttackChance()
    {
        int attackChance;
        if (distanceFromPlayer >= 4.5f && lastAttack != 1 && lastAttack != 4)
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
        int attackFour = FourthAttackChance();
        IEnumerable<int> attackOneRange = Enumerable.Range(1, attackOne);
        IEnumerable<int> attackTwoRange = Enumerable.Range(attackOne, attackTwo);
        IEnumerable<int> attackThreeRange = Enumerable.Range(attackOne + attackTwo, attackThree);
        IEnumerable<int> attackFourRange = Enumerable.Range(attackOne + attackTwo + attackThree, attackFour);

        int sumOfAll = attackOne + attackTwo + attackThree + attackFour;

        int randomNumber = Random.Range(1, sumOfAll);
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
        else if (attackFourRange.Contains(randomNumber))
        {
            finalNumber = 4;
        }
        else
        {
            Debug.Log($"Attackhoice error: RNGNum: {randomNumber}");
        }

        return finalNumber;
    }
}
