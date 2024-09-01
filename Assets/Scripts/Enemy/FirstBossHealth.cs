using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstBossHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private Slider healthSlider;
    public int currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthSlider.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = currentHealth;
        if (currentHealth <= 0)
        {
            // I wonder how to implement death for enemy and player, probably reference currenthealth toward other script and not do it here
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player Projectile")
        {
            currentHealth--;
        }
    }
}
