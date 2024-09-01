using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgradeItem : MonoBehaviour
{
    [SerializeField] private int upgradeAmount;
    public void UpgradeHealth()
    {
        FindAnyObjectByType<PlayerHealth>().maxHealth += upgradeAmount;
        FindAnyObjectByType<PlayerHealth>().currentHealth += upgradeAmount;
    }
}
