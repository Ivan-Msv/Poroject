using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Sprite fullHPSprite, halfHPSprite, lowHPSprite, noHPSprite;
    private SpriteRenderer playerSprite;
    public int maxHealth;
    public int currentHealth;
    [SerializeField] private float iFrameDuration;
    private PlayerHealthUI healthUI;
    private bool hitBoxActive = true;
    private Color originalColor;

    void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        originalColor = transform.GetComponent<SpriteRenderer>().color;
        healthUI = GetComponentInChildren<PlayerHealthUI>();
        healthUI.DrawHearts();
    }

    public void Update()
    {
        SpriteManager();
    }
    private IEnumerator ActivateIFrames()
    {
        hitBoxActive = false;
        transform.GetComponent<SpriteRenderer>().color = Color.white; // placeholder
        yield return new WaitForSeconds(iFrameDuration);
        transform.GetComponent<SpriteRenderer>().color = originalColor;
        hitBoxActive = true;
    }

    public void TakeDamage()
    {
        switch (currentHealth)
        {
            case int n when n > 1:
                AudioManager.instance.PlaySound("playerhit");
                break;
            case 1:
                AudioManager.instance.PlaySound("playerdeath");
                break;
        }
        currentHealth--;
        healthUI.DrawHearts();
        StartCoroutine(ActivateIFrames());
    }

    public void HealToFullHP()
    {
        currentHealth = maxHealth;
        healthUI.DrawHearts();
    }
    public void UpgradeHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        healthUI.DrawHearts();
    }

    private void SpriteManager()
    {
        if (currentHealth <= 0)
        {
            playerSprite.sprite = noHPSprite;
        }
        else if (currentHealth <= maxHealth / 3)
        {
            playerSprite.sprite = lowHPSprite;
        }
        else if (currentHealth <= maxHealth / 2)
        {
            playerSprite.sprite = halfHPSprite;
        }
        else if (currentHealth == maxHealth)
        {
            playerSprite.sprite = fullHPSprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy Projectile") && hitBoxActive && gameObject)
        {
            TakeDamage();
        }
    }
}
