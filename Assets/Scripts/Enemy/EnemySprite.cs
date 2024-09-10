using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySprite : MonoBehaviour
{
    [SerializeField] private Sprite[] suicidalSprites, genericSprites, tankSprites;
    private SpriteRenderer spriteRenderer;
    private EnemyType enemyType;
    void Start()
    {
        enemyType = GetComponent<EnemyBehavior>().Type;
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetEnemySprite();
    }

    private void SetEnemySprite()
    {
        switch (enemyType)
        {
            case EnemyType.Suicidal:
                spriteRenderer.sprite = suicidalSprites[Random.Range(0, suicidalSprites.Length)];
                break;
            case EnemyType.Generic:
                spriteRenderer.sprite = genericSprites[Random.Range(0, genericSprites.Length)];
                break;
            case EnemyType.Tank:
                spriteRenderer.sprite = tankSprites[Random.Range(0, tankSprites.Length)];
                break;
            default:
                spriteRenderer.sprite = genericSprites[Random.Range(0, genericSprites.Length)];
                break;
        }
    }
}

