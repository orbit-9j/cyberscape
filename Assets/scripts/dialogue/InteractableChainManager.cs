using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableChainManager : MonoBehaviour
{

    public Collider2D[] triggers; //order in which triggers should be interacted with

    public int currentTrigger = 0; //public for debug purposes


    private static InteractableChainManager Instance; //do i need it to be a singleton? i don't refer to it from anywhere

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("more than one chain manager manager on the scene");
        }
        Instance = this;
    }

    public static InteractableChainManager GetInstance()
    {
        return Instance;
    }

    void Update()
    {
        if (triggers.Length > 0)
        {
            if (triggers[currentTrigger].gameObject.GetComponent<DialogueTrigger>().hasInteracted)
            {
                if (triggers[currentTrigger].gameObject.GetComponent<DialogueTrigger>().isDisablable)
                {
                    triggers[currentTrigger].enabled = false; //disable current object's trigger
                }

                currentTrigger++;

                if (currentTrigger < triggers.Length)
                {
                    triggers[currentTrigger].enabled = true; //enable next in chain object's trigger
                }

            }
        }
    }
}
