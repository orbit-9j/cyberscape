using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BruteForcePuzzleController : MonoBehaviour
{
    /* this is the puzzle that demonstrates a brute force attack against a caesar cipher. the player needs to rotate a wheel with 26 segments, 
    representing the 26 letters of the english alphabet, to find the correct key (offset) to decode the message */

    private GameManager gameManager;
    [SerializeField] private CipherSceneManager sceneManager;
    public RectTransform wheel; // Reference to the wheel Image component - it is a placeholder that will need to be replaced
    private int segmentNumber = 26;
    [SerializeField] private int correctKey;
    [SerializeField] private TextMeshProUGUI number;
    [SerializeField] private TextMeshProUGUI ciphertext;
    private float currentRotation;
    private float segmentAngle;
    private float currentSegment;

    //private string originalPlaintext = "Hey door, are you free tomorrow after six pm? I was thinking we could hang out, if you know what I mean.";
    private string originalCiphertext;

    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private string knotName = "main";

    public bool completed;
    private bool acceptInput;
    private bool canExit;

    public void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
        gameManager.playerMoves = false;
        correctKey = sceneManager.encryptionKey;
        originalCiphertext = sceneManager.ciphertext;
        acceptInput = true;
        gameObject.SetActive(true);
        completed = false;
        canExit = false;
        currentSegment = 1;
        currentRotation = 0f;
        number.text = "key: 1";
        segmentAngle = 360f / segmentNumber;
    }

    private void Update()
    {
        GetInput();
    }

    public void GetInput()
    {
        if (acceptInput) //rotates the wheel 
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                currentRotation += segmentAngle;
                RotateWheel(currentRotation);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                currentRotation -= segmentAngle;
                RotateWheel(currentRotation);
            }

            if (Input.GetKeyDown(KeyCode.Return) && canExit)
            {
                gameManager.FlashText(ciphertext, Color.white, Color.green);
                completed = true;
                acceptInput = false;
                gameManager.playerMoves = true;
                gameObject.SetActive(false);
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                DialogueManager.GetInstance().JumpToKnot(knotName);
            }
        }
    }

    private void RotateWheel(float angle)
    {
        wheel.rotation = Quaternion.Euler(0f, 0f, angle);

        //wrapping the number around the wheel logic:
        currentSegment = (currentRotation / segmentAngle) + 1;
        currentSegment = gameManager.mod((currentSegment - 1 + segmentNumber), segmentNumber) + 1;

        number.text = "key: " + Mathf.RoundToInt(currentSegment).ToString(); //update key text

        ciphertext.text = gameManager.Decipher(originalCiphertext, Mathf.RoundToInt(currentSegment)); //update deciphered text

        if (Mathf.RoundToInt(currentSegment) == correctKey)
        {
            // ciphertext.color = Color.green;
            canExit = true;
        }
    }
}



