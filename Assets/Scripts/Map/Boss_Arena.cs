using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Arena : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private float speed;
    public bool fightActive;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fightActive)
        {
            CloseDoor();
        }
        else
        {
            OpenDoor();
        }
    }
    private void CloseDoor()
    {
        if (door.transform.localScale.x < 1)
        {
            door.transform.localScale = new Vector3(Mathf.Lerp(door.transform.localScale.x, 1.5f, speed * Time.deltaTime), 1, 1);
        }
    }
    private void OpenDoor()
    {
        if (door.transform.localScale.x > 0.1f)
        {
            door.transform.localScale = new Vector3(Mathf.Lerp(door.transform.localScale.x, 0, speed * Time.deltaTime), 1, 1);
        }
    }
}
