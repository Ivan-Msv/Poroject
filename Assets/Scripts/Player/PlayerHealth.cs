using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    [SerializeField] private float iFrameDuration;
    private bool hitBoxActive = true;
    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        originalColor = transform.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator ActivateIFrames()
    {
        hitBoxActive = false;
        transform.GetComponent<SpriteRenderer>().color = Color.white; // placeholder
        yield return new WaitForSeconds(iFrameDuration);
        transform.GetComponent<SpriteRenderer>().color = originalColor;
        hitBoxActive = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy Projectile") && hitBoxActive && gameObject)
        {
            currentHealth--;
            StartCoroutine(ActivateIFrames());
        }
    }
}
