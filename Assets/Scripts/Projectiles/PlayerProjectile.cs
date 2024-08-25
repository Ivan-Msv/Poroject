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
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            Destroy(gameObject);
        }
    }
    public void SetDirection(Vector3 spawnPoint, Vector3 direction)
    {
        moveDirection = direction;
        startPos = spawnPoint;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy Projectile") && !collision.CompareTag("Player") && !collision.CompareTag("Player Projectile"))
        {
            Destroy(gameObject);
        }
    }
}
