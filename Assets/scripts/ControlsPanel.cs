using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPanel : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject panel;
    private bool open;

    void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
        open = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (open)
            {
                panel.SetActive(false);
                open = false;
                gameManager.playerMoves = true;
            }
            else
            {
                panel.SetActive(true);
                open = true;
                gameManager.playerMoves = false;
            }
        }
    }
}
