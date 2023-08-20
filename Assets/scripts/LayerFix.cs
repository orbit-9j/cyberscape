using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerFix : MonoBehaviour
{
    //simulate an object being in front or behind the player
    private SpriteRenderer spriteRenderer;
    private GameObject player;
    private int sortOrder;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        sortOrder = spriteRenderer.sortingOrder;
    }

    void LateUpdate()
    {
        if (player.transform.position.y > transform.position.y)
        {
            spriteRenderer.sortingOrder = sortOrder + 20;
        }
        else
        {
            spriteRenderer.sortingOrder = sortOrder;
        }
    }
}
