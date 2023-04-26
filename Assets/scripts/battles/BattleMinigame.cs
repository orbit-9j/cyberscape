using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMinigame : MonoBehaviour
{
    protected GameManager gameManager; //protected allows inherited classes to see the variable, unlike private
    protected float timeLeft = 100.0f;
    /*  protected bool isTimerRunning = false; */
    public bool winState;
    public bool minigameEnded;
    [SerializeField] protected GameObject screenObject;
    [SerializeField] protected GameObject textPrefab; //i think i only need it for soceng battle
    [SerializeField] private GameObject minigameUI;
    protected bool acceptInput;

    public Timer timer;

    [SerializeField] private BattleController panel;
    private bool isCountingDown = false;
    //public Text timerText;

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

        //StartCoroutine(UpdateTimer());
        if (!isCountingDown)
        {
            StartCoroutine(CheckTime());
        }

    }

    protected IEnumerator CheckTime()
    {
        isCountingDown = true;
        while (timer.timerRunning)
        {
            yield return null;
        }
        Debug.Log("timer stopped");
        EndMinigame();
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
            damage = damage + (panel.player.streak * 3); //check streak and determine multiplier
        }

        damage += Random.Range(0, 3);
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

        int damage = CalculateDamage();
        //panel.opponent.TakeDamage(10); //debug
        if (damage != 0) //play damage animation if damage has been dealt
        {
            panel.opponent.TakeDamage(damage);
            Debug.Log("enemy damage: " + damage);
        }

        minigameUI.SetActive(false);
        Debug.Log("disabling minigame ui");
        minigameEnded = true;
        panel.currentCharacter.minigamePlaying = false;
    }
}
