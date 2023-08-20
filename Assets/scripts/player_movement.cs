using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    private GameManager gameManager;

    //https://www.youtube.com/watch?v=whzomFgjT50 09/02/2023
    public float moveSpeed = 7f;

    public Rigidbody2D rb;
    public Animator animator;

    Vector2 movement;

    void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //need to figure out the walking animation. the player needs to keep looking in the direction they were walking in rather than defaulting to looking rights
        /* animator.SetFloat("Horizontal", movement.x);
        //animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude); */

        if (movement.x < 0)
        {
            // Set the "Walk Left" animation
            animator.SetFloat("Horizontal", -1f);
            animator.SetFloat("Speed", Mathf.Abs(movement.x));
        }
        else if (movement.x > 0)
        {
            // Set the "Walk Right" animation
            animator.SetFloat("Horizontal", 1f);
            animator.SetFloat("Speed", Mathf.Abs(movement.x));
        }
        else if (movement.y != 0)
        {
            // Set the "Idle" animation
            animator.SetFloat("Speed", 0f);
        }
        else
        {
            // Set the "Idle" animation
            animator.SetFloat("Speed", 0f);
        }
    }

    void FixedUpdate()
    {
        //https://www.youtube.com/watch?v=vY0Sk93YUhA 12/02/2023
        if (DialogueManager.GetInstance().dialogueIsPlaying || DialogueManager.GetInstance().puzzlePlaying || !gameManager.playerMoves)
        {
            return;
        }
        //--------------------------------------

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
