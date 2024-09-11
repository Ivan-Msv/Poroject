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
    private float spawnDecayTime;
    private float spawnSpeed;

    void Awake()
    {
        spawnScale = transform.localScale;
        spawnSpeed = mainProjectileSpeed;
        transform.localScale = Vector3.zero;
        spawnDecayTime = mainProjectileDecayTime;
    }

    private void OnDisable()
    {
        transform.localScale = Vector3.zero;
        mainProjectileSpeed = spawnSpeed;
        mainProjectileDecayTime = spawnDecayTime;
    }

    // Update is called once per frame
    void Update()
    {
        mainProjectileDecayTime -= Time.deltaTime;
        if (mainProjectileDecayTime <= 0)
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
            // explosion into deleting object
            ProjectileManager.instance.SpawnRotatingProjectiles(this.transform, explodingAmount, 0, timeTillDecay - mainProjectileDecayTime, true, false, 70, explodingProjectileSpeed);
            ProjectilePoolSystem.instance.ReturnToPool(gameObject);
        }
    }
    private void SpawnProjectile()
    {
        if (transform.localScale.x <= spawnScale.x)
        {
            transform.localScale += spawnScale * (decaySpeed / 3) * Time.deltaTime;
        }
    }

    public void SetDirection(Vector3 parentPos, Vector3 axis, float projectileMoveSpeed, float decayTime)
    {
        parentPosition = parentPos;
        directionAxis = axis;
        timeTillDecay = decayTime;
        mainProjectileSpeed = projectileMoveSpeed;
    }
}
