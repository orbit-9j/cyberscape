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

        Debug.Log("end of turn");
        if (player.remainingHealth > 0 && enemy.remainingHealth > 0)
        {
            StartCoroutine(StartTurn());
        }

        else
        {
            Debug.Log("health check running");
            if (player.remainingHealth > enemy.remainingHealth)
            {
                playerWin = true;
                //win code
                //delete enemy, next interaction
            }
            else
            {
                playerWin = false;
                //loss code
                //exit and retry enemy
            }

            EndBattle();
        }
    }

    void Update()
    {
        /* Debug.Log("update method running");
        if (player.remainingHealth <= 0 || enemy.remainingHealth <= 0)
        {
            if (player.remainingHealth > enemy.remainingHealth)
            {
                playerWin = true;
                //win code
                //delete enemy, next interaction
            }
            else
            {
                playerWin = false;
                //loss code
                //exit and retry enemy
            }

            EndBattle();
        } */
    }

    private void EndBattle()
    {
        gameManager.playerMoves = true;
        gameObject.SetActive(false);
    }
}
