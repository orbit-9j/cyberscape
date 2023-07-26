using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMinigame : MonoBehaviour
{
    protected GameManager gameManager; //protected allows inherited classes to see the variable, unlike private
    [SerializeField] protected float timeLeft = 100.0f;
    public bool winState;
    public bool minigameEnded;
    [SerializeField] protected GameObject screenObject;
    [SerializeField] protected GameObject minigameUI;
    protected bool acceptInput;

    public Timer timer;

    [SerializeField] private BattleManager panel;


    public void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
        minigameUI.SetActive(true);
        minigameEnded = false;
        acceptInput = true;
        winState = false;

        timer.totalTime = timeLeft;
        timer.timerRunning = true;
        timer.StartTimer();
    }
    public virtual void ResetMinigame() //start is only called once when the script loads so i can't reset values from Start(). the reset method resets all the minigame variables and is called once it finishes so it can start again with fresh variables
    {

    }

    protected void Update()
    {
        if (!timer.timerRunning)
        {
            winState = false;
            EndMinigame();
        }
    }

    protected void dealDamageDefence()
    {
        //handle streak
        if (winState)
        {
            panel.playerStreak++;
        }
        else
        {
            panel.playerStreak = 0;
        }
        panel.streakText.text = "streak: " + panel.playerStreak;

        //handle enemy damage
        int enemyDamage = 0;
        if (winState) //if won, deal damage, otherwise damage is 0
        {
            enemyDamage += 5;
            if (timer.remainingTime != 0)
            {
                enemyDamage += Mathf.RoundToInt((timer.remainingTime / timer.totalTime) * 15); //time-proportionate damage
            }
        }
        enemyDamage = enemyDamage + (panel.playerStreak * 2); //add more damage with a higher streak
        enemyDamage += Random.Range(0, 3);
        panel.TakeDamage(enemyDamage, ref panel.enemyRemainingHealth, panel.enemyHealthBar, panel.enemyHighlight);

        //handle player damage
        int playerDamage = 0;
        if (!winState) //if the player lost, maximise damage
        {
            playerDamage += 15;
        }
        playerDamage += Random.Range(0, 3);
        panel.TakeDamage(playerDamage, ref panel.playerRemainingHealth, panel.playerHealthBar, panel.playerHighlight);
    }

    protected void dealDamageAttack()
    {
        //handle enemy damage
        int enemyDamage = 5;
        enemyDamage += (Random.Range(0, 3) + panel.playerStreak);
        panel.TakeDamage(enemyDamage, ref panel.enemyRemainingHealth, panel.enemyHealthBar, panel.enemyHighlight);

        //handle player damage
        int playerDamage = 1;
        playerDamage += Random.Range(0, 2);
        panel.TakeDamage(playerDamage, ref panel.playerRemainingHealth, panel.playerHealthBar, panel.playerHighlight);
    }

    protected void EndMinigame()
    {
        acceptInput = false;
        timer.timerRunning = false;

        if (panel.defence)
        {
            dealDamageDefence();
        }
        else
        {
            dealDamageAttack();
        }

        minigameEnded = true;
        ResetMinigame();
        minigameUI.SetActive(false);
        panel.minigamePlaying = false;
    }
}
