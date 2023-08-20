using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//for interactions with no dialogue
public class SimpleInteraction : Interactable
{
    [SerializeField] private UnityEvent interactAction;

    protected override void DoAction()
    {
        interactAction.Invoke();
    }
}
