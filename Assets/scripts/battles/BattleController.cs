using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BattleController : MonoBehaviour
{
    /* this script controls the flow of battle and the states of the parties involved. the exact architecture of the various battle scripts
    is still in the works, these are some ideas on how it could be done: */

    private GameManager gameManager;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject enemyObject;
    public BattlePlayer player;
    private BattleEnemy enemy;

    public BattleCharacter opponent;
    public BattleCharacter currentCharacter;

    public TextMeshProUGUI streakText;
    private bool playerWin = false;

    //option to flee and try again

    public void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
        gameManager.playerMoves = false;
        gameObject.SetActive(true);
        player = playerObject.GetComponent<BattlePlayer>();
        enemy = enemyObject.GetComponent<BattleEnemy>();

        //debug
        //enemy.TakeDamage(5);
        //Debug.Log("health left: " + enemy.totalHealth + "/60");

        player.turn = true; //start off the battle with player's turn
        opponent = enemy;

        StartCoroutine(StartTurn());
    }

    private IEnumerator StartTurn()
    {
        if (player.turn)
        {
            player.StartTurn();

            while (!player.minigame.minigameEnded)
            {
                yield return null;
            }

            player.EndTurn();
            enemy.turn = true;
            opponent = player;
            currentCharacter = enemy;
        }
        else
        {
            enemy.StartTurn();
            while (!enemy.minigame.minigameEnded)
            {
                yield return null;
            }
            enemy.EndTurn();
            player.turn = true;
            opponent = enemy;
            currentCharacter = player;
        }

        //add exit condition (while player or enemy alive)
        StartCoroutine(StartTurn());
    }

    void Update()
    {
        //keep track of time - give time bar own script?
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

    /* private void HandleBar(float totalWidth, int oldValue, int valueChange)
    {
        //set new bar width proportionate to oldValue-valueChange and the total width of the object
    } */

    private void EndBattle()
    {
        gameManager.playerMoves = true;
    }
}
