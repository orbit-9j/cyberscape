using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCharacter : MonoBehaviour
{
    //why are they all public?
    /* This class is the parent class for BattleEnemy and battlePlayer and it defines some methods and variables that the player and enemy 
    will have in common, like TakeDamage(), StartTurn() totalHealth, and turn. The BattleController class will access it during the battle 
    to keep track of states and variables of the parties involved*/

    public int totalHealth;
    public int remainingHealth;
    public Image highlight; //yellow background when it's the character's turn, red background when they're taking damage
    public bool turn; //keeps track of whether it's the gameObject's turn

    public BattleMinigame minigame;
    public GameObject panel;

    public string characterName;
    public bool minigamePlaying;

    public HealthBar healthBar; //https://www.youtube.com/watch?v=BLfNP4Sc_iA  06/04/2023



    public virtual void Start()
    {
        BattleController battleController = panel.GetComponent<BattleController>();

        healthBar.SetMaxHealth(totalHealth); //https://www.youtube.com/watch?v=BLfNP4Sc_iA  06/04/2023

        highlight.color = Color.yellow;
        highlight.enabled = false;

    }

    void Update()
    {
        if (minigamePlaying)
        {
            if (minigame.minigameEnded)
            {
                EndTurn();
            }
        }

    }

    public virtual void TakeDamage(int damage)
    {
        remainingHealth -= damage;
        StartCoroutine(FlashHighlight(Color.red)); //flashes red for visual feedback of a character's state
        //update health bar
        healthBar.SetHealth(remainingHealth); //https://www.youtube.com/watch?v=BLfNP4Sc_iA  06/04/2023
    }

    private IEnumerator FlashHighlight(Color colour) //taking damage animation
    {
        if (!turn)
        {
            highlight.enabled = true;
        }

        highlight.color = colour;
        yield return new WaitForSeconds(0.3f);
        highlight.color = Color.yellow;

        if (!turn)
        {
            highlight.enabled = false;
        }
    }

    public virtual void StartTurn()
    {
        highlight.enabled = true; //highlights the character whose turn it is
        turn = true;
        //battleController.StartCoroutine(WaitForMinigameToComplete(minigame));
        minigame.Start();
    }

    public virtual void EndTurn()
    {
        highlight.enabled = false; //removes highlight from character as it is no longer their turn
        turn = false;
    }
}
