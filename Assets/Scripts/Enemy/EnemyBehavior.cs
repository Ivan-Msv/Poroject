using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;
public class EnemyBehavior : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        int randomValue = Random.Range(0, 180);
        Vector3 playerDirection = (ProjectileManager.instance.player.transform.position - transform.position).normalized;
        Quaternion rotationAngle = Quaternion.Euler(0, 0, MathF.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProjectileManager.instance.SpawnProjectile(this.transform, 5, 5, 1, playerDirection, rotationAngle);
            ProjectileManager.instance.SpawnRotatingProjectiles(this.transform, 10, 0, 1f, true, false, randomValue, 0.5f);
        }
    }
}
