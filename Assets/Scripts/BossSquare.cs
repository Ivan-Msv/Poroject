using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSquare : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.parent.position, Vector3.forward, 20 * Time.deltaTime);
    }
}
