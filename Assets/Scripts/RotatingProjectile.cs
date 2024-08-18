using System.Collections;
using System.Collections.Generic;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeTillDecay -= Time.deltaTime;
        if (timeTillDecay <= 0 || Vector3.Distance(transform.position, rotationPos) > 12 && moveForward)
        {
            DeleteProjectile();
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
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector3 direction, Vector3 rotationTransform, bool shouldMoveForward)
    {
        moveDirection = direction;
        rotationPos = rotationTransform;
        moveForward = shouldMoveForward;
    }
}
