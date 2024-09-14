using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public int keyItemState = 0;
    [SerializeField] private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (DialogueManager.instance.dialogueActive || !RespawnManager.instance.CanMove)
        {
            return;
        }
        PlayerMovement();
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Time.timeScale = 5;
        }
        
    }

    private void PlayerMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        float vertical = Input.GetAxisRaw("Vertical") * moveSpeed * Time.fixedDeltaTime;
        Vector2 oldPos = rb.position;

        rb.MovePosition(new Vector2(oldPos.x + horizontal, oldPos.y + vertical));
    }
}
