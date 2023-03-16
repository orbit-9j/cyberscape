using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleEnemy : BattleCharacter
{
    [SerializeField] private Image characterImage;

    [SerializeField] private Sprite newSprite; // Reference to the new sprite that will replace the current sprite, according to the type of enemy the player encountered. I may not do it this way in the finalised version, this is a draft

    public BattleEnemy()
    {
        //set stats for the enemy
        totalHealth = 60;
        remainingHealth = totalHealth;
    }

    void Start()
    {
        base.Start();
        characterImage.sprite = newSprite;
    }

    /* void Update()
    {

    } */
}
