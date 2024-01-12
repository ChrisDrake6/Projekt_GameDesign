using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        LayerMask mask = LayerMask.GetMask("Player");
        if (Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y - transform.lossyScale.y / 2), new Vector2(2, 2), 0, Vector2.down, 2, mask))
        {
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Walls");
        }
        else
        {
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Obstacles");
        }
    }
}
