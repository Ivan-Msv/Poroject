using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AxisProjectile : MonoBehaviour
{
    [SerializeField] private float decaySpeed = 3;
    private float moveSpeed;
    private float moveSpeedMultiplier;
    private float timeTillDecay;
    private Vector3 parentPosition;
    private Vector3 directionAxis;
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
        if (timeTillDecay <= 0 || Vector3.Distance(new Vector3(0, transform.position.y, 0), new Vector3(0, parentPosition.y, 0)) > 12)
        {
            DeleteProjectile();
        }
        else
        {
            SpawnProjectile();
        }

        transform.position += directionAxis * moveSpeed * Time.deltaTime;
        moveSpeed += moveSpeedMultiplier * Time.deltaTime;
    }

    private void DeleteProjectile()
    {
        transform.localScale *= 1 - decaySpeed * Time.deltaTime;
        if (transform.localScale.x <= 0.2f)
        {
            Destroy(gameObject);
        }
    }
    private void SpawnProjectile()
    {
        if (transform.localScale.x <= spawnScale.x)
        {
            transform.localScale += spawnScale * decaySpeed * Time.deltaTime;
        }
    }
    public void SetDirection(Vector3 parentPos, Vector3 axis, float movementSpeed, float movespeedAcceleration, float decayTime)
    {
        parentPosition = parentPos;
        directionAxis = axis;
        timeTillDecay = decayTime;
        moveSpeed = movementSpeed;
        moveSpeedMultiplier = movespeedAcceleration;
    }
}
