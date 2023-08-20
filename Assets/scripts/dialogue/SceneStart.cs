using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStart : MonoBehaviour
{

    [SerializeField] protected TextAsset inkJSON;
    //[SerializeField] private string knotName;

    protected virtual void Start()
    {
        JumpToFile(inkJSON);
    }

    protected void JumpToFile(TextAsset inkJSON)
    {
        if (DialogueManager.GetInstance() != null)
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            DialogueManager.GetInstance().JumpToKnot("main");
        }
    }

}
