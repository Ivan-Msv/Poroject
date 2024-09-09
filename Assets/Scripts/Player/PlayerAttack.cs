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
        if (Time.timeScale == 0)
        {
            return;
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
