using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExplodingProjectile : MonoBehaviour
{
    [SerializeField] private GameObject rotatingProjectile;

    [Header("Main Projectile Settings")]
    [SerializeField] private float mainProjectileSpeed;
    [SerializeField] private float mainProjectileSpeedMultiplier;
    [SerializeField] private float mainProjectileDecayTime;
    [SerializeField] private float mainProjectileRotationSpeed;
    [SerializeField] private float decaySpeed;
    [Space]
    [Header("Exploding Projectiles' Settings")]
    [SerializeField] private int explodingAmount;
    [SerializeField] private float explodingProjectileSpeed;
    private Vector3 parentPosition;
    private Vector3 directionAxis;
    private Vector3 spawnScale;
    private float timeTillDecay;
    // Start is called before the first frame update
    void Start()
    {
        spawnScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        mainProjectileDecayTime -= Time.deltaTime;
        if (mainProjectileDecayTime <= 0 || Vector3.Distance(new Vector3(0, transform.position.y, 0), new Vector3(0, parentPosition.y, 0)) > 12)
        {
            DeleteProjectile();
        }
        else
        {
            SpawnProjectile();
        }

        transform.position += directionAxis * mainProjectileSpeed * Time.deltaTime;
        mainProjectileSpeed += mainProjectileSpeedMultiplier * Time.deltaTime;
    }

    private void DeleteProjectile()
    {
        transform.localScale *= 1 - decaySpeed * Time.deltaTime;
        if (transform.localScale.x <= 0.2f)
        {
            SpawnExplosion();
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

    private void SpawnExplosion()
    {
        float angleStep = 360f / explodingAmount;
        float angle = 0f;

        for (int i = 0; i < explodingAmount; i++)
        {
            Vector3 currentPos = transform.position;

            float projectileXDirection = currentPos.x + Mathf.Sin((angle * Mathf.PI) / 180);
            float projectileYDirection = currentPos.y + Mathf.Cos((angle * Mathf.PI) / 180);

            Vector3 projectileVector = new Vector3(projectileXDirection, projectileYDirection, 0);
            Vector3 projectileMoveDirection = (projectileVector - transform.position).normalized;

            GameObject projectileInstance = Instantiate(rotatingProjectile, currentPos, Quaternion.identity);
            projectileInstance.GetComponent<RotatingProjectile>().SetDirection(projectileMoveDirection, currentPos, true, timeTillDecay - mainProjectileDecayTime, 70, explodingProjectileSpeed);

            angle += angleStep;
        }
    }
    public void SetDirection(Vector3 parentPos, Vector3 axis, float decayTime)
    {
        parentPosition = parentPos;
        directionAxis = axis;
        timeTillDecay = decayTime;
    }
}
