using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private GameObject rotatingProjectile, axisProjectile, explodingProjectile;
    public static ProjectileManager instance;
    public GameObject player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        player = FindAnyObjectByType<PlayerHealth>().gameObject;
    }

    private void Start()
    {
        //Create new projectiles into the pool
        ProjectilePoolSystem.instance.InitNewPool(rotatingProjectile, 100);
        ProjectilePoolSystem.instance.InitNewPool(axisProjectile, 100);
        ProjectilePoolSystem.instance.InitNewPool(explodingProjectile, 10);
    }

    public void SpawnRotatingProjectiles(Transform spawnObject, int amount, float radius, float attackDuration, bool shouldMoveForward, bool aroundThePlayer, float rotationSpeed = 70, float projectileSpeed = 0)
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
                    currentPos = spawnObject.position;
                    break;
            }

            if (radius != 0)
            {
                float projectileXDirection = currentPos.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
                float projectileYDirection = currentPos.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

                Vector3 projectileVector = new Vector3(projectileXDirection, projectileYDirection, 0);
                Vector3 projectileMoveDirection = (projectileVector - currentPos).normalized;

                GameObject projectileInstance = ProjectilePoolSystem.instance.GetObject(rotatingProjectile, projectileVector, transform.rotation);
                projectileInstance.GetComponent<RotatingProjectile>().SetDirection(projectileMoveDirection, currentPos, shouldMoveForward, attackDuration, rotationSpeed);
            }
            else
            {
                float projectileXDirection = currentPos.x + Mathf.Sin((angle * Mathf.PI) / 180);
                float projectileYDirection = currentPos.y + Mathf.Cos((angle * Mathf.PI) / 180);

                Vector3 projectileVector = new Vector3(projectileXDirection, projectileYDirection, 0);
                Vector3 projectileMoveDirection = (projectileVector - currentPos).normalized;

                GameObject projectileInstance = ProjectilePoolSystem.instance.GetObject(rotatingProjectile, currentPos, transform.rotation);
                projectileInstance.GetComponent<RotatingProjectile>().SetDirection(projectileMoveDirection, currentPos, shouldMoveForward, attackDuration, rotationSpeed, projectileSpeed);
            }

            angle += angleStep;
        }
    }

    public void SpawnProjectile(Transform spawnObject, int amount, float projectileSpeed, float projectileDuration, Vector3 projectileMoveDirection, Quaternion? rotation = null)
    {
        Quaternion newRotation = rotation ?? axisProjectile.transform.rotation;

        for (int i = 0; i < amount; i++)
        {
            GameObject newProjectile = ProjectilePoolSystem.instance.GetObject(axisProjectile, spawnObject.position, newRotation);
            newProjectile.GetComponent<AxisProjectile>().SetDirection(transform.position, projectileMoveDirection, projectileSpeed, 2, projectileDuration);
        }
    }
}
