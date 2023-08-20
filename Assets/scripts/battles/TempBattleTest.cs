using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempBattleTest : BattleMinigame
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        winState = true;
        EndMinigame();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
