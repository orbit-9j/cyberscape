using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMinigame : MonoBehaviour
{
    protected GameManager gameManager; //protected allows inherited classes to see the variable, unlike private
    protected float timeLeft = 5.0f;
    /*  protected bool isTimerRunning = false; */
    public bool winState = false;
    public bool minigameEnded = false;
    [SerializeField] protected GameObject screenObject;
    [SerializeField] protected GameObject textPrefab;
    [SerializeField] private GameObject minigameUI;
    protected bool acceptInput;

    public Timer timer;

    [SerializeField] private BattleController panel;

    //public Text timerText;

    public void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
        minigameUI.SetActive(true);
        minigameEnded = false;
        acceptInput = true;

        timer.totalTime = timeLeft;
        timer.timerRunning = true;
        timer.StartTimer();

        //StartCoroutine(UpdateTimer());
    }

    protected virtual void Update()
    {
        if (!timer.timerRunning)
        {
            EndMinigame();
        }
    }

    protected int CalculateDamage()
    {
        int damage = 0;

        //check if timer ran out

        if (winState) //if won, deal damage, otherwise damage is 0
        {
            damage += 10;
        }

        if (panel.player.turn)
        {
            damage = damage + (panel.player.streak * 2); //check streak and determine multiplier
        }

        //also add a random number between like 0 and 3 to make the damage unpredictable. use method from gamemanager

        return damage;
    }

    protected void EndMinigame()
    {
        acceptInput = false;
        timer.timerRunning = false;

        if (panel.player.turn) //update player streak
        {
            if (winState)
            {
                panel.player.streak++;
            }
            else
            {
                panel.player.streak = 0;
            }
            panel.streakText.text = "streak: " + panel.player.streak;
        }

        if (CalculateDamage() != 0) //play damage animation if damage has been dealt
        {
            panel.opponent.TakeDamage(CalculateDamage());
        }

        minigameEnded = true;
        minigameUI.SetActive(false);
        panel.currentCharacter.minigamePlaying = false;
    }
}
