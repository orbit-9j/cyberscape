using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePlayer : BattleCharacter
{
    public int streak;

    public BattlePlayer()
    {
        //set stats for the player
        streak = 0;
        totalHealth = 80;
        remainingHealth = totalHealth;
        //character name = player name
    }
}
