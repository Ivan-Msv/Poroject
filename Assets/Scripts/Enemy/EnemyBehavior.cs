using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDirection = (ProjectileManager.instance.player.transform.position - transform.position).normalized;
        Quaternion rotationAngle = Quaternion.Euler(0, 0, MathF.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ProjectileManager.instance.SpawnProjectile(this.transform, 5, 5, 1, playerDirection, rotationAngle);
            ProjectileManager.instance.SpawnRotatingProjectiles(this.transform, 10, 0, 1f, true, false, 0, 0.5f);
        }
    }
}
