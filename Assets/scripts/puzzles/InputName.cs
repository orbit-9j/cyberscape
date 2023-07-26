using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputName : Puzzle
{
    [SerializeField] private Button enterButton;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private TextMeshProUGUI placeholderText;
    [SerializeField] private TMP_InputField inputField;

    public void Start()
    {
        base.Start();

        gameObject.SetActive(true);

        StartCoroutine(AutoFocusInputField());
    }
    IEnumerator AutoFocusInputField()
    {
        // Wait for one frame
        yield return null;

        // Automatically focus the input field
        inputField.Select();
        inputField.ActivateInputField();
    }

    void Update()
    {
        if (acceptInput)
        {
            string inputText = inputField.text;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("button pressed");
                gameManager.PressUnpressButton(enterButton, defaultSprite, pressedSprite);
                if (inputField.text != "")
                {
                    gameManager.playerName = inputField.text;
                }
                else
                {
                    gameManager.playerName = "Mortimer";
                }
                Debug.Log(gameManager.playerName);
                FinishPuzzle();
                ContinueDialogue();
                gameObject.SetActive(false);
            }


        }
    }
}
