using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleCharacter : MonoBehaviour
{
    /* This class is the parent class for BattleEnemy and battlePlayer and it defines some methods and variables that the player and enemy 
    will have in common, like TakeDamage(), StartTurn() totalHealth, and turn. The BattleController class will access it during the battle 
    to keep track of states and variables of the parties involved*/

    public int totalHealth;
    public int remainingHealth;
    public Image highlight; //yellow background when it's the character's turn, red background when they're taking damage
    public bool turn; //keeps track of whether it's the gameObject's turn
    public string topic; //ciphers, social engineering, or physical security. decides which type of minigame the battle will have


    /* general idea: a list of minigames, one for each topic. the minigame to be played will be selected depending on the battle topic. each 
    minigame class has attack and defend methods, called by the player. the minigames will inherit from the Minigame class, which will define
    the Attack() and Defend() methods, as well as some variables */
    //public Minigame ciperMinigame;
    //public Minigame socengMinigame;
    //public Minigame physical Minigame;


    public virtual void Start()
    {
        highlight.color = Color.yellow;
        highlight.enabled = false;

        //idea for how the minigame selection will work
        /* switch (topic)
        {
            case "ciphers":
                //select cipher minigame

        } */
    }

    /*  void Update()
     {

     } */

    public virtual void TakeDamage(int damage)
    {
        remainingHealth -= damage;
        StartCoroutine(FlashHighlight(Color.red)); //flashes red for visual feedback of a character's state
        //update health bar
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
    }

    public virtual void EndTurn()
    {
        highlight.enabled = false; //removes highlight from character as it is no longer their turn
        turn = false;
    }

    public virtual void Die()
    {
        //todo
    }
}
