using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    private List<GameObject> enemies = new List<GameObject>();
    private Transform oldParent;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }


    public void DisableEnemy(GameObject enemy, EnemyHealth healthScript)
    {
        oldParent = enemy.transform.parent; // only works if they're all under the same parent otherwise it gets weird (my assumption)
        enemy.transform.parent = this.transform;
        enemy.SetActive(false);
        enemies.Add(enemy);
        healthScript.HealToFull();
    }

    public void RespawnAllEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject enemy = enemies[i];
            enemy.transform.parent = oldParent;
            enemy.transform.position = enemy.GetComponent<EnemyBehavior>().EnemySpawnPoint;
            enemy.SetActive(true);
            enemies.Remove(enemy);
        }
    }
}
