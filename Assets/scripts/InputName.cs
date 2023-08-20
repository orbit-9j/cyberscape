using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputName : MonoBehaviour
{

    private GameManager gameManager;
    public bool completed;
    private bool acceptInput;

    [SerializeField] private Button enterButton;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private TextMeshProUGUI inputText;
    [SerializeField] private TextMeshProUGUI placeholderText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private string knotName;

    public void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
        gameObject.SetActive(true);
        acceptInput = true;
        completed = false;
        gameManager.playerMoves = false;
        /*  inputField.Select();
         inputField.ActivateInputField(); */
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
                acceptInput = false;
                completed = true;
                gameManager.playerMoves = true;
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                DialogueManager.GetInstance().JumpToKnot(knotName);
                gameObject.SetActive(false);
            }


        }
    }
}
