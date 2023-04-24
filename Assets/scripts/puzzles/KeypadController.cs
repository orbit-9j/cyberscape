using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class KeypadController : MonoBehaviour
{
    /* this script will be used in the phishing scene as the last puzzle. it allows the user to enter the code they got from the phishing
    puzzle to unlock the door to the next scene */

    private GameManager gameManager;
    [SerializeField] private GameObject door;

    [SerializeField] private Button[] buttons; // Array of all the keypad buttons
    [SerializeField] private Button esc;

    private Sprite[] defaultSprites; //default sprites of all the buttons
    private Sprite[] pressedSprites; // pressed sprites of all the buttons
    private Sprite escDefaultSprite;
    private Sprite escPressedSprite;

    [SerializeField] private TextMeshProUGUI pinText;
    public string correctPin;

    public bool completed = false;

    private void Start()
    {
        gameObject.SetActive(true);
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
        gameManager.playerMoves = false;

        //store the default and pressed sprites of all the buttons
        defaultSprites = new Sprite[buttons.Length];
        pressedSprites = new Sprite[buttons.Length];
        pinText.text = "";
        completed = false;

        //assign sprites to each button
        for (int i = 0; i < buttons.Length; i++)
        {
            defaultSprites[i] = buttons[i].image.sprite;
            pressedSprites[i] = buttons[i].spriteState.pressedSprite;
        }
        escDefaultSprite = esc.image.sprite;
        escPressedSprite = esc.spriteState.pressedSprite;
    }

    private void Update()
    {
        //check if any keypad key is pressed
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pinText.text = "";
                pinText.color = Color.white;
                esc.image.sprite = escPressedSprite;
                //StartCoroutine(ResetButtonSprite(esc, buttonPressDuration, false));
                //ResetButtonSprite(false, esc);
                gameManager.UnpressButton(esc, escDefaultSprite);
            }

            for (int i = 0; i < 10; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                {
                    //Debug.Log("Key " + (i) + " was pressed.");
                    int buttonIndex = i;

                    buttons[buttonIndex].image.sprite = pressedSprites[buttonIndex];

                    if (pinText.text.Length < 4)
                    {
                        pinText.text += buttonIndex.ToString();
                    }

                    if (pinText.text.Length == 4)
                    {
                        if (pinText.text == correctPin)
                        {
                            pinText.color = Color.green;
                            completed = true;
                            gameManager.playerMoves = true;
                            Destroy(door);
                            gameObject.SetActive(false);
                        }
                        else { pinText.color = Color.red; }
                    }

                    //StartCoroutine(ResetButtonSprite(buttons[buttonIndex], buttonPressDuration, true));
                    //ResetButtonSprite(true, buttons[buttonIndex]);
                    gameManager.UnpressButton(buttons[buttonIndex], defaultSprites[System.Array.IndexOf(buttons, buttons[buttonIndex])]);
                }
            }
        }
    }
}



