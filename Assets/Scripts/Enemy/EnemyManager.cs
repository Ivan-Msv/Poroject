using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Transform enemyFolder;
    public static EnemyManager instance;
    private List<GameObject> enemies = new List<GameObject>();
    private List<GameObject> deadEnemies = new List<GameObject>();
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        foreach (Transform enemy in enemyFolder)
        {
            enemies.Add(enemy.gameObject);
        }
    }


    public void DisableEnemy(GameObject enemy, EnemyHealth healthScript)
    {
        enemy.transform.parent = this.transform;
        enemy.SetActive(false);
        deadEnemies.Add(enemy);
        enemies.Remove(enemy);
        healthScript.HealToFull();
    }

    public void RespawnAllEnemies()
    {
        List<GameObject> temp = new List<GameObject>(deadEnemies);
        for (int i = 0; i < temp.Count; i++)
        {
            GameObject enemy = temp[i];
            enemy.transform.parent = enemyFolder;
            enemy.transform.position = enemy.GetComponent<EnemyBehavior>().EnemySpawnPoint;
            enemy.SetActive(true);
            deadEnemies.Remove(enemy);
            enemies.Add(enemy);
        }
        // But also heal and reset the enemies that were alive
        foreach (GameObject enemy in enemies)
        {
            enemy.transform.position = enemy.GetComponent<EnemyBehavior>().EnemySpawnPoint;
            enemy.GetComponent<EnemyHealth>().HealToFull();
        }
    }
}
