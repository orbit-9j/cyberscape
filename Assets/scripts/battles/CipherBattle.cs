using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CipherBattle : MonoBehaviour
{

    /* plan:
    inspiration: that rootkit minigame in scrutinized, where there's a timer to move a slider and you have to listen out for a beep and move the slider in the beep position before the timer runs out
    that, but with a wheel and you have to press submit when you see the correct plaintext 
    
    i may need to split it into 2 separate classes that inherit from this class so it's easier to do the update method but tbh idek what the defend attack method will be lol*/

    [SerializeField] private BruteForcePuzzleController puzzle;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        puzzle.GetInput();
    }

    public /* override */ void Attack()
    {

    }

    public /* override */ void Defend()
    {

    }
}
