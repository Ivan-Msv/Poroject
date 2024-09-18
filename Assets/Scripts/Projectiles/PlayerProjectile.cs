using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileSpeedMultiplier;
    [SerializeField] private float projectileMaxDistance;
    private Vector3 moveDirection;
    private Vector3 startPos;

    private float startSpeed;
    private Vector3 startScale;

    void Awake()
    {
        startScale = transform.localScale;
        startSpeed = projectileSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveDirection * projectileSpeed * Time.deltaTime;
        projectileSpeed += projectileSpeedMultiplier * Time.deltaTime;

        if (Vector3.Distance(startPos, transform.position) > projectileMaxDistance)
        {
            DeleteProjectile();
        }
    }

    private void DeleteProjectile()
    {
        transform.localScale *= 1 - 2 * Time.deltaTime;
        if (transform.localScale.x <= 0.01f)
        {
            ProjectilePoolSystem.instance.ReturnToPool(gameObject);
        }
    }
    public void SetDirection(Vector3 spawnPoint, Vector2 direction)
    {
        moveDirection = direction.normalized;
        startPos = spawnPoint;

        transform.localScale = startScale;
        projectileSpeed = startSpeed;
    }

    private void AudioSetting(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            AudioManager.instance.PlaySound("EnemyHit");
        }
        else if (collision.CompareTag("BreakableWall"))
        {
            AudioManager.instance.PlaySound("BreakableImpact");
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSetting(collision);
        if (collision.CompareTag("Enemy") || collision.CompareTag("Wall") || collision.CompareTag("BreakableWall"))
        {
            ProjectilePoolSystem.instance.ReturnToPool(gameObject);
        }
    }
}
