using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject playerProjectile;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float projectileFrequency;
    private float timer;

    void Update()
    {
        if (Time.timeScale == 0 || !RespawnManager.instance.Alive)
        {
            return;
        }
        // debug
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Time.timeScale = 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            Time.timeScale = 3f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProjectileManager.instance.DisableAllProjectiles();
        }

        timer += Time.deltaTime;
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (new Vector3(mousepos.x, mousepos.y, 0) - transform.parent.position).normalized;

        if (Input.GetMouseButton(0) && timer >= projectileFrequency)
        {
            AudioManager.instance.PlaySound("PlayerProjectile");
            GameObject newProjectile = ProjectilePoolSystem.instance.GetObject(playerProjectile, spawnPoint.position, transform.rotation);
            newProjectile.GetComponent<PlayerProjectile>().SetDirection(spawnPoint.position, direction);
            timer = 0;
        }
    }
}
