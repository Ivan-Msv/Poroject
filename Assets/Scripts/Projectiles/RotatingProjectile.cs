using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RotatingProjectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float moveSpeedMultiplier;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationSpeedMultiplier;
    [SerializeField] private float timeTillDecay;
    [SerializeField] private float decaySpeed;
    private bool moveForward;
    private Vector3 moveDirection;
    private Vector3 rotationPos;
    private Vector3 spawnScale;
    private float spawnSpeed;

    void Awake()
    {
        spawnScale = transform.localScale;
        spawnSpeed = moveSpeed;
        transform.localScale = Vector3.zero;
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
        moveSpeed = spawnSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        timeTillDecay -= Time.deltaTime;
        if (timeTillDecay <= 0)
        {
            DeleteProjectile();
        }
        else
        {
            SpawnProjectile();
        }

        Move();
        Rotate();
    }

    private void Move()
    {
        if (moveForward)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            moveSpeed += moveSpeedMultiplier * Time.deltaTime;
        }
    }

    private void Rotate()
    {
        transform.RotateAround(rotationPos, Vector3.forward, rotationSpeed * Time.deltaTime);
        if (rotationSpeed >= 0 && moveForward)
        {
            rotationSpeed -= rotationSpeedMultiplier * Time.deltaTime;
        }
    }

    private void DeleteProjectile()
    {
        transform.localScale *= 1 - decaySpeed * Time.deltaTime;
        if (transform.localScale.x <= 0.1f)
        {
            ProjectilePoolSystem.instance.ReturnToPool(gameObject);
        }
    }

    private void SpawnProjectile()
    {
        if (transform.localScale.x <= spawnScale.x)
        {
            transform.localScale += spawnScale * (decaySpeed / 3)* Time.deltaTime;
        }
    }

    public void SetDirection(Vector3 direction, Vector3 rotationTransform, bool shouldMoveForward, float decayTime, float rotateSpeed, float speed = 0)
    {
        moveDirection = direction;
        rotationPos = rotationTransform;
        moveForward = shouldMoveForward;
        timeTillDecay = decayTime;
        rotationSpeed = rotateSpeed;

        if (speed != 0)
        {
            moveSpeed = speed;
        }
    }
}
