using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;
    [SerializeField] Fade deathUI;
    [SerializeField] Fade fadeBackground;
    [SerializeField] Boss_Arena arena;
    [field: SerializeField] public bool CanMove { get; private set; } = true;
    private bool isRespawning = false;
    private PlayerHealth player;
    public GameObject playerRespawnPoint { get; private set; }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        player = FindAnyObjectByType<PlayerHealth>();
    }

    void Update()
    {
        CanMove = player.currentHealth > 0;
        DeathScreen();
    }

    public void SetPlayerRespawn(GameObject RespawnpointObject)
    {
        playerRespawnPoint = RespawnpointObject;
    }
    private void DeathScreen()
    {
        if (!CanMove)
        {
            deathUI.StartFadeIn();
            if (Input.GetKeyDown(KeyCode.E) && !isRespawning)
            {
                StartCoroutine(RespawnAllObjects());
            }
        }
    }
    private IEnumerator RespawnAllObjects()
    {
        isRespawning = true;
        fadeBackground.StartFadeIn();
        yield return new WaitForSeconds(fadeBackground.FadeSpeed);
        ArenaRestart();
        RespawnPlayer();
        deathUI.GetComponent<CanvasGroup>().alpha = 0; // very crude
        fadeBackground.StartFadeOut();
        isRespawning = false;
    }
    private void RespawnPlayer()
    {
        if (playerRespawnPoint != null)
        {
            player.transform.position = new Vector2(playerRespawnPoint.transform.position.x, playerRespawnPoint.transform.position.y - 1);
        }
        else // Set default cave spawn point
        {
            player.transform.position = new Vector2(-8, -5);
        }
        player.currentHealth = player.maxHealth;
        player.HealToFullHP();
    }
    private void ArenaRestart()
    {
        arena.ResetArena();
    }
}
