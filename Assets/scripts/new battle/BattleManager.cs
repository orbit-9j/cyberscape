using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleManager : MonoBehaviour
{
    //minigame vars
    private GameManager gameManager;
    [SerializeField] private BattleMinigame defenceMinigame;
    [SerializeField] private BattleMinigame attackMinigame;
    public bool defence; //defence/attack aka playerTurn
    public bool minigamePlaying;
    private bool playerWin;
    private bool battleStarted;

    //player vars
    [SerializeField] private int playerTotalHealth = 80;
    public int playerRemainingHealth;
    public HealthBar playerHealthBar; //https://www.youtube.com/watch?v=BLfNP4Sc_iA  06/04/2023
    public int playerStreak;
    public TextMeshProUGUI streakText;
    public Image playerHighlight; //flashes red when character takes damage

    //enemy vars
    public int enemyTotalHealth = 60;
    public int enemyRemainingHealth;
    public HealthBar enemyHealthBar; //https://www.youtube.com/watch?v=BLfNP4Sc_iA  06/04/2023
    public Image enemyHighlight;
    [SerializeField] private GameObject enemy;

    //next interaction
    [SerializeField] private GameObject trigger;


    public void Start()
    {
        //general panel start logic
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
        gameManager.playerMoves = false;
        gameObject.SetActive(true);
        attackMinigame.enabled = false;
        defenceMinigame.enabled = true;
        defence = true;
        minigamePlaying = false;
        playerWin = false;
        battleStarted = false;

        //initialise player vars
        playerRemainingHealth = playerTotalHealth;
        playerHealthBar.SetMaxHealth(playerTotalHealth); //https://www.youtube.com/watch?v=BLfNP4Sc_iA  06/04/2023
        playerHighlight.enabled = false;
        playerStreak = 0;
        streakText.text = "Streak: " + playerStreak;

        //initialise enemy vars
        enemyRemainingHealth = enemyTotalHealth;
        enemyHealthBar.SetMaxHealth(enemyTotalHealth); //https://www.youtube.com/watch?v=BLfNP4Sc_iA  06/04/2023
        enemyHighlight.enabled = false;

        if (!battleStarted)
        {
            battleStarted = true;
            StartCoroutine(StartTurn());
        }

    }

    //make both parties take damage per turn
    private IEnumerator StartTurn()
    {
        if (defence)
        {
            //Debug.Log("defence");
            defenceMinigame.enabled = true;
            defenceMinigame.Start();

            while (!defenceMinigame.minigameEnded)
            {
                yield return null;
            }

            defenceMinigame.enabled = false;
            defence = false;
        }
        else
        {
            //Debug.Log("attack");
            attackMinigame.enabled = true;
            attackMinigame.Start();

            while (!attackMinigame.minigameEnded)
            {
                yield return null;
            }

            attackMinigame.enabled = false;
            defence = true;
        }

        if (playerRemainingHealth > 0 && enemyRemainingHealth > 0)
        {
            StartCoroutine(StartTurn());
        }
        else
        {
            if (playerRemainingHealth > enemyRemainingHealth)
            {
                playerWin = true;
                Destroy(enemy); //delete enemy object - it has been defeated
                trigger.GetComponent<BoxCollider2D>().enabled = true; //enable the next interaction (e.g. the elevator button)
                EndBattle();
            }
            else
            {
                playerWin = false;
                gameObject.SetActive(false); //exit battle and try again
                EndBattle();
            }

            EndBattle();
        }
    }

    void Update()
    {
    }

    private void EndBattle()
    {
        gameManager.playerMoves = true;
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage, ref int remainingHealth, HealthBar healthBar, Image highlight)
    {
        remainingHealth -= damage;

        StartCoroutine(FlashHighlight(Color.red, highlight)); //flashes red for visual feedback of a character's state

        healthBar.SetHealth(remainingHealth); //https://www.youtube.com/watch?v=BLfNP4Sc_iA  06/04/2023

    }

    private IEnumerator FlashHighlight(Color colour, Image highlight) //taking damage animation
    {
        highlight.enabled = true;
        highlight.color = colour;
        yield return new WaitForSeconds(0.3f);
        highlight.enabled = false;
    }
}

