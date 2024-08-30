using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortcutDoor : MonoBehaviour
{
    [SerializeField] private GameObject[] leftSide;
    [SerializeField] private GameObject[] rightSide;
    [SerializeField] private float speed;
    public bool shouldOpen;
    void Update()
    {
        Open();
    }

    public void Open()
    {
        if (shouldOpen)
        {
            foreach (var tile in leftSide)
            {
                tile.transform.position = Vector3.MoveTowards(tile.transform.position, new Vector3(27.505f, 33, 0), speed * Time.deltaTime);
            }

            foreach (var tile in rightSide)
            {
                tile.transform.position = Vector3.MoveTowards(tile.transform.position, new Vector3(28.503f, 28.3f, 0), speed * Time.deltaTime);
            }
        }
    }
}
