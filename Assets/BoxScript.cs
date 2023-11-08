using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxScript : EnemyBase
{
    [SerializeField] Sprite evilBoxSprite;
    [SerializeField] GameObject evilDecoy;

    private void Start()
    {
        if (Random.Range(1,20) <= 1)
        {
            spriteRenderer.sprite = evilBoxSprite;
        }
    }

    public void DieLate()
    {
        if (spriteRenderer.sprite == evilBoxSprite)
        {
            Instantiate(evilDecoy, transform.position, Quaternion.identity);
        }
    }
}
