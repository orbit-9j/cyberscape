using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalSceneManager : MonoBehaviour
{
    private GameManager gameManager;

    //list or individual?

    //player name: dusk, endgame, the end
    //console name: endgame, the end

    [SerializeField] private List<GameObject> triggers;

    void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();

        foreach (GameObject trigger in triggers)
        {
            trigger.GetComponent<DialogueTrigger>().variableNames = new List<string>(){
                "playerName", "consoleName"
            };

            trigger.GetComponent<DialogueTrigger>().variableNames = new List<string>(){
                gameManager.playerName, gameManager.consoleName
            };
        }
    }

}
