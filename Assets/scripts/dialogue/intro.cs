using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intro : MonoBehaviour
{

    [SerializeField] private TextAsset inkJSON;
    //[SerializeField] private string knotName;

    void Start()
    {
        if (DialogueManager.GetInstance() != null)
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            DialogueManager.GetInstance().JumpToKnot("main");
        }

    }

}
