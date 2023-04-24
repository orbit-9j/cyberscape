using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected GameObject visualCue;
    protected bool playerInRange;

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    protected virtual void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying)
        {

            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                DoAction();
            }

        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collider)
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    protected virtual void DoAction()
    {

    }
}
