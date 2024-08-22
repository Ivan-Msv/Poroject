using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject playerProjectile;
    [SerializeField] private Transform spawnPoint;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (new Vector3(mousepos.x, mousepos.y, 0) - transform.parent.position).normalized;

        if (Input.GetMouseButtonDown(0))
        {
            GameObject newProjectile = Instantiate(playerProjectile, spawnPoint.position, transform.rotation);
            newProjectile.GetComponent<PlayerProjectile>().SetDirection(spawnPoint.position, direction);
        }
    }
}
