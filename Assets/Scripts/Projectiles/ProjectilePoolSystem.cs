using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolSystem : MonoBehaviour
{
    // Couldn't create this on my own...
    // https://www.youtube.com/watch?v=B86sH_II3MY
    public static ProjectilePoolSystem instance;
    private Dictionary<GameObject, Queue<GameObject>> poolDictionary = new Dictionary<GameObject, Queue<GameObject>>();
    private Dictionary<GameObject, GameObject> pooledProjectileOrigin = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject GetObject(GameObject objectToSpawn, Vector3 objectPosition, Quaternion objectRotation)
    {
        GameObject objectToGet = poolDictionary[objectToSpawn].Dequeue();

        if (poolDictionary[objectToSpawn].Count == 0)
        {
            CreateNewProjectile(objectToSpawn);
        }

        objectToGet.transform.parent = null;
        objectToGet.transform.position = objectPosition;
        objectToGet.transform.rotation = objectRotation;
        objectToGet.SetActive(true);

        return objectToGet;
    }

    public void InitNewPool(GameObject prefab, int poolSize)
    {
        poolDictionary[prefab] = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            CreateNewProjectile(prefab);
        }
    }

    private void CreateNewProjectile(GameObject prefab)
    {
        GameObject newObject = Instantiate(prefab, transform);
        newObject.SetActive(false);

        poolDictionary[prefab].Enqueue(newObject);
        pooledProjectileOrigin[newObject] = prefab;
    }

    public void ReturnToPool(GameObject objectToReturn)
    {
        GameObject originalObject = pooledProjectileOrigin[objectToReturn];

        objectToReturn.SetActive(false);
        objectToReturn.transform.parent = this.transform;
        poolDictionary[originalObject].Enqueue(objectToReturn);
    }
}
