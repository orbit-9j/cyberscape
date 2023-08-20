using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    protected GameManager gameManager;
    protected bool completed;
    protected bool acceptInput;
    [SerializeField] protected TextAsset inkJSON;
    [SerializeField] protected string knotName;

    // Start is called before the first frame update
    protected void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
        gameManager.playerMoves = false;
        acceptInput = true;
        completed = false;

        //add gameObject.SetActive(true);?
    }

    protected void ContinueDialogue()
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        DialogueManager.GetInstance().JumpToKnot(knotName);
    }

    protected void ContinueDialogueVariables(List<string> varNames, List<string> varValues)
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        DialogueManager.GetInstance().JumpToKnot(knotName, varNames, varValues);
    }

    protected void FinishPuzzle()
    {
        acceptInput = false;
        completed = true;
        gameManager.playerMoves = true;
    }
}
