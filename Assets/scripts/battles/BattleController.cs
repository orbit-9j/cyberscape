using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : MonoBehaviour
{
    /* this script controls the flow of battle and the states of the parties involved. the exact architecture of the various battle scripts
    is still in the works, these are some ideas on how it could be done: */

    private bool playerTurn = true;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject enemyObject;
    private BattlePlayer player;
    private BattleEnemy enemy;

    private int streak = 0;
    private bool playerWin = false;


    //option to flee and try again

    public void Start()
    {
        gameObject.SetActive(true);
        player = playerObject.GetComponent<BattlePlayer>();
        enemy = enemyObject.GetComponent<BattleEnemy>();
        player.StartTurn();

        //debug
        /* enemy.TakeDamage(5);
        Debug.Log("health left: " + enemy.totalHealth + "/60"); */
    }

    void Update()
    {
        //keep track of time - give time bar own script?

        if (Input.GetKeyDown(KeyCode.Space)) //should later be controlled with code rather than key input, on the basis of waiting for a turn to finish. maybe use pace bar to attack?
        {
            //need to check if character has completed their turn before being able to switch

            if (playerTurn)
            {
                playerTurn = false;
                player.EndTurn();
                enemy.StartTurn();

            }
            else
            {
                playerTurn = true;
                player.StartTurn();
                enemy.EndTurn();
            }
        }
    }

    private void HandleStreak() //where do i call it?
    {
        //if time left > 0 and minigame has not been completed, 
        //damage to enemy: damage + streak*someValue (or (damage + streak) * value?)
        //damage to player: damage - streak*someValue (or (damage + streak) * value?)

        //if time left > 0 and minigame has been completed, win = true, streak++, ManageWinLoss();
        //else, win = false, streak = 0, ManageWinLoss();
    }

    private void HandleTimer(float timeAllowed)
    {
        //private float timeLeft; get time left from the minigame script?

        //if  time left <=0 : lose state
        //else:
        //update time bar text
        //update time bar width
    }

    private void ManageWinLoss()
    {
        if (playerWin)
        {
            //win code
        }
        else
        {
            //lose code
        }
    }

    private void HandleBar(float totalWidth, int oldValue, int valueChange)
    {
        //set new bar width proportionate to oldValue-valueChange and the total width of the object
    }
}
