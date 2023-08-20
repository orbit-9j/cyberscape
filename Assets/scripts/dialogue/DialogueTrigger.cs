using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=vY0Sk93YUhA 12/02/2023
public class DialogueTrigger : Interactable
{
    /* this script is attached to an interactable object to allow dialogue to play upon interaction. i have modified it to be able to disable the 
    trigger so an object can't be interacted with again using the same dialogue script; the ability to destroy the object completely 
    after interaction; and the ability to control what story knot the trigger should jump to*/

    private GameManager gameManager;

    [Header("Ink JSON")]
    public TextAsset inkJSON;
    [SerializeField] private string knotName;

    //my addition---------
    [SerializeField] private bool destroy; // if ticked, this object cannot be interacted with again
    [SerializeField] private string afterGoTo; //if specified, will prompt the dialogue in this knot after the interaction (can be used in conjunction with the destroy functionality to trigger dialogue after destruction)
    /*  [SerializeField] private bool interactable = true; */
    [SerializeField] private bool autoplay;

    [SerializeField] private List<GameObject> nextInteractions;

    //variables relating to the chain of interaction
    public bool hasInteracted = false;
    private bool hasStartedSpeaking = false;
    public bool isDisablable = false; //determines if the trigger should be disabled after interaction

    public List<string> variableNames;
    public List<string> variableValues;
    //------------
    void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
    }

    private void EnableNextInteractable()
    {
        if (hasInteracted)
        {
            /*  if (isDisablable)
             {
                 Destroy(gameObject);
             } */

            if (nextInteractions != null)
            {
                foreach (GameObject obj in nextInteractions)
                {
                    if (obj.name == "trigger")
                    {
                        obj.GetComponent<BoxCollider2D>().enabled = true;
                    }
                    else if (obj.name == "barrier")
                    {
                        Destroy(obj);
                    }
                    else
                    {
                        obj.SetActive(true);
                    }
                }
                //Debug.Log("enabled objects");
            }
        }

    }

    protected override void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying && gameManager.playerMoves/*  && interactable */)
        {
            if (autoplay && !hasInteracted) //currently loops, needs something to only play it once
            {
                EnterDialogue();
                hasInteracted = true;
                EnableNextInteractable();
            }
            else
            {
                visualCue.SetActive(true);
                //if (InputManager.GetInstance().GetInteractPressed())
                if (Input.GetKeyDown(KeyCode.E))
                {
                    EnterDialogue();
                    hasInteracted = true;
                    EnableNextInteractable();
                }
            }
        }
        else
        {
            visualCue.SetActive(false);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collider)
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
                    transform.parent.gameObject.SetActive(false); //destroy (disable) parent
                }
                else if (isDisablable)
                {
                    Destroy(gameObject); //disable (destroy) the trigger
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

    private void EnterDialogue()
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        if (variableNames != null && variableValues != null)
        {
            DialogueManager.GetInstance().JumpToKnot(knotName, variableNames, variableValues);
        }
        else
        {
            DialogueManager.GetInstance().JumpToKnot(knotName);
        }

        hasStartedSpeaking = true;
    }

}
