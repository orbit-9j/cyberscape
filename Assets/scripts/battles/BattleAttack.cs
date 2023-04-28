using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleAttack : BattleMinigame
{
    //a simple script to attack the enemy. since the defence minigames are the focus of the battle, this script is to fill in the gaps in the battle flow
    [SerializeField] private TextMeshProUGUI attackText;

    void Start()
    {
        base.Start();

        //wait till time runs out (2 seconds)
    }
}
