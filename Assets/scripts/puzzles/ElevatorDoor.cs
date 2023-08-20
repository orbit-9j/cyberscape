using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer elevatorImage;
    [SerializeField] private Sprite openDoorSprite;

    [SerializeField] private BoxCollider2D collider;
    public void ChangeSprite()
    {
        elevatorImage.sprite = openDoorSprite;
        collider.enabled = false;
    }
}
