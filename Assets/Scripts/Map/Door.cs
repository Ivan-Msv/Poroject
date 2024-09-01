using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject[] doors;
    [SerializeField] private Vector3[] doorClosedPos;
    [SerializeField] private Vector3[] doorOpenPos;
    [SerializeField] private float speed;
    public bool shouldOpen;
    public bool shouldClose;
    void Update()
    {
        DoorSystem();
    }

    public void DoorSystem()
    {
        if (shouldOpen)
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].transform.position = Vector3.MoveTowards(doors[i].transform.position, doorOpenPos[i], speed * Time.deltaTime);
            }
        }
        else if (shouldClose)
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].transform.position = Vector3.MoveTowards(doors[i].transform.position, doorClosedPos[i], speed * Time.deltaTime);
            }
        }
    }
}
