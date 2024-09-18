using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConeRotation : MonoBehaviour
{
    void Update()
    {
        if (Time.timeScale == 0 || !RespawnManager.instance.Alive)
        {
            return;
        }
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }
}
