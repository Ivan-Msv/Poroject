using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    private List<PlayerHeart> hearts = new List<PlayerHeart>();
    private PlayerHealth health;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponentInParent<PlayerHealth>();
    }

    private void ClearHearts()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        hearts = new List<PlayerHeart>();
    }

    private void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab, this.transform);
        PlayerHeart heartComponent = newHeart.GetComponent<PlayerHeart>();
        heartComponent.SetHeartImage(HeartState.Empty);
        hearts.Add(heartComponent);
    }

    public void DrawHearts()
    {
        ClearHearts();

        for (int i = 0; i < health.maxHealth; i++)
        {
            CreateEmptyHeart();
        }

        for (int i = 0; i < health.currentHealth; i++)
        {
            hearts[i].SetHeartImage(HeartState.Full);
        }
    }
}
