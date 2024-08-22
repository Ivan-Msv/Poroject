using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        float vertical = Input.GetAxisRaw("Vertical") * moveSpeed * Time.fixedDeltaTime;
        Vector2 oldPos = rb.position;

        rb.MovePosition(new Vector2(oldPos.x + horizontal, oldPos.y + vertical));
    }
}
