using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject playerProjectile;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float projectileFrequency;
    private float timer;

    private void Start()
    {
        ProjectilePoolSystem.instance.InitNewPool(playerProjectile, 10);
    }

    void Update()
    {
        timer += Time.deltaTime;
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (new Vector3(mousepos.x, mousepos.y, 0) - transform.parent.position).normalized;

        if (Input.GetMouseButton(0) && timer >= projectileFrequency)
        {
            GameObject newProjectile = ProjectilePoolSystem.instance.GetObject(playerProjectile, spawnPoint.position, transform.rotation);
            newProjectile.GetComponent<PlayerProjectile>().SetDirection(spawnPoint.position, direction);
            timer = 0;
        }
    }
}
