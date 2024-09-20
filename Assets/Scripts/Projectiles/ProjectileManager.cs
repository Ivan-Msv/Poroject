using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private GameObject rotatingProjectile, axisProjectile, explodingProjectile, playerProjectile;
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
        ProjectilePoolSystem.instance.InitNewPool(playerProjectile, 10);
        ProjectilePoolSystem.instance.InitNewPool(rotatingProjectile, 100);
        ProjectilePoolSystem.instance.InitNewPool(axisProjectile, 100);
        ProjectilePoolSystem.instance.InitNewPool(explodingProjectile, 10);
    }

    public void SpawnRotatingProjectiles(Transform spawnObject, int amount, float radius, float attackDuration, bool shouldMoveForward, float rotationSpeed = 70, float projectileSpeed = 0, bool altAngle = false)
    {
        float angleStep = 360f / amount;
        float angle = 0;
        if (altAngle)
        {
            angle += angleStep / 2;
        }
        else
        {
            angle += angleStep * 2;
        }
        for (int i = 0; i < amount; i++)
        {
            Vector3 currentPos = spawnObject.transform.position;

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

                GameObject projectileInstance = ProjectilePoolSystem.instance.GetObject(rotatingProjectile, currentPos, spawnObject.transform.rotation);
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
    public void SpawnExplodingProjectile(Transform spawnObject, int amount, float projectileSpeed, float projectileDuration, Vector3 projectileMoveDirection, Quaternion? rotation = null)
    {
        Quaternion newRotation = rotation ?? axisProjectile.transform.rotation;

        for (int i = 0; i < amount; i++)
        {
            GameObject newProjectile = ProjectilePoolSystem.instance.GetObject(explodingProjectile, spawnObject.position, newRotation);
            newProjectile.GetComponent<ExplodingProjectile>().SetDirection(transform.position, projectileMoveDirection, projectileSpeed, projectileDuration);
        }
    }
    public void SpawnEnemyRotatingProjectiles(Transform spawnObject, bool setAsParent, int amount, float radius, float attackDuration, float rotationSpeed = 70, float projectileSpeed = 0)
    {
        float angleStep = 360f / amount;
        float angle = 0f;
        for (int i = 0; i < amount; i++)
        {
            Vector3 currentPos = spawnObject.position;

            if (radius != 0)
            {
                float projectileXDirection = currentPos.x + Mathf.Sin((angle * Mathf.PI) / 180) * radius;
                float projectileYDirection = currentPos.y + Mathf.Cos((angle * Mathf.PI) / 180) * radius;

                Vector3 projectileVector = new Vector3(projectileXDirection, projectileYDirection, 0);
                Vector3 projectileMoveDirection = (projectileVector - currentPos).normalized;

                GameObject projectileInstance = ProjectilePoolSystem.instance.GetObject(rotatingProjectile, projectileVector, transform.rotation);
                projectileInstance.GetComponent<RotatingProjectile>().SetDirection(projectileMoveDirection, currentPos, true, attackDuration, rotationSpeed);
                if (setAsParent)
                {
                    projectileInstance.transform.parent = spawnObject;
                }
            }
            else
            {
                float projectileXDirection = currentPos.x + Mathf.Sin((angle * Mathf.PI) / 180);
                float projectileYDirection = currentPos.y + Mathf.Cos((angle * Mathf.PI) / 180);

                Vector3 projectileVector = new Vector3(projectileXDirection, projectileYDirection, 0);
                Vector3 projectileMoveDirection = (projectileVector - currentPos).normalized;

                GameObject projectileInstance = ProjectilePoolSystem.instance.GetObject(rotatingProjectile, currentPos, transform.rotation);
                projectileInstance.GetComponent<RotatingProjectile>().SetDirection(projectileMoveDirection, currentPos, true, attackDuration, rotationSpeed, projectileSpeed);
                if (setAsParent)
                {
                    projectileInstance.transform.parent = spawnObject;
                }
            }

            angle += angleStep;
        }
    }

    public void DisableAllProjectiles()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject gameObject in allObjects)
        {
            if (gameObject.GetComponent<RotatingProjectile>() != null || gameObject.GetComponent<AxisProjectile>() || gameObject.GetComponent<ExplodingProjectile>())
            {
                if (gameObject.activeInHierarchy)
                {
                    ProjectilePoolSystem.instance.ReturnToPool(gameObject);
                }
            }
        }
    }
}
