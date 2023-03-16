using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

/* this script was a test idea and as far as i know isn't being used anywhere in the game any more. still, i'm not deleting it in case
it breaks something haha */

public class NextDialogueTrigger : MonoBehaviour
{
    public TextAsset inkJSON;
    [SerializeField] private string knotName;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            DialogueManager.GetInstance().JumpToKnot(knotName);
            Destroy(gameObject);
        }
    }
}
