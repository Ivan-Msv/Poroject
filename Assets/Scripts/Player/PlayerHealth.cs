using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    [SerializeField] private float iFrameDuration;
    private PlayerHealthUI healthUI;
    private bool hitBoxActive = true;
    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        originalColor = transform.GetComponent<SpriteRenderer>().color;
        healthUI = GetComponentInChildren<PlayerHealthUI>();
        healthUI.DrawHearts();
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy Projectile") && hitBoxActive && gameObject)
        {
            TakeDamage();
        }
    }
}
