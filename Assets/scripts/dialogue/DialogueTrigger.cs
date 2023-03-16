using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=vY0Sk93YUhA 12/02/2023
public class DialogueTrigger : MonoBehaviour
{
    /* this script is attached to an interactable object to allow dialogue to play upon interaction. i have modified it to be able to disable the 
    trigger so an object can't be interacted with again using the same dialogue script; the ability to destroy the object completely 
    after interaction; and the ability to control what story knot the trigger should jump to*/

    [Header("Visual Cue")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    public TextAsset inkJSON;
    [SerializeField] private string knotName;
    private bool playerInRange;

    //my addition---------
    [SerializeField] private bool destroy; // if ticked, this object cannot be interacted with again
    [SerializeField] private string afterGoTo; //if specified, will prompt the dialogue in this knot after the interaction (can be used in conjunction with the destroy functionality to trigger dialogue after destruction)
    [SerializeField] private bool interactable = true;

    //variables relating to the chain of interaction
    public bool hasInteracted = false;
    private bool hasStartedSpeaking = false;
    public bool isDisablable = false; //determines if the trigger should be disabled after interaction
    //------------

    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying && interactable)
        {
            visualCue.SetActive(true);
            //if (InputManager.GetInstance().GetInteractPressed())
            if (Input.GetKeyDown(KeyCode.E))
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                DialogueManager.GetInstance().JumpToKnot(knotName);
                hasStartedSpeaking = true;
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

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerInRange = false;
            //own code --------------
            if (hasStartedSpeaking)
            {
                hasInteracted = true;
                if (destroy)
                {
                    transform.parent.gameObject.SetActive(false);
                }
            }
            if (afterGoTo != null)
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                DialogueManager.GetInstance().JumpToKnot(afterGoTo);
            }
            //--------------------
        }
    }
}
