using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BruteForcePuzzleController : MonoBehaviour
{
    /* this is the puzzle tat demonstrates a brute force attack against a caesar cipher. the player needs to rotate a wheel with 26 segments, 
    representing the 26 letters of the english alphabet, to find the correct key (offset) to decode the message */

    private GameManager gameManager;
    public RectTransform wheel; // Reference to the wheel Image component - it is a placeholder that will need to be replaced
    private int segmentNumber = 26;
    [SerializeField] private int correctKey = 12; //will need to be randomly selected in the future
    [SerializeField] private TextMeshProUGUI number;
    [SerializeField] private TextMeshProUGUI ciphertext;
    private float currentRotation;
    private float segmentAngle;
    private float currentSegment;

    //private string originalPlaintext = "Hey door, are you free tomorrow after six pm? I was thinking we could hang out, if you know what I mean.";
    private string originalCiphertext = "Tqk paad, mdq kag rdqq fayaddai mrfqd euj by? U ime ftuzwuzs iq oagxp tmzs agf, ur kag wzai itmf U yqmz."; //will need to be randomly selected in the future

    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private string knotName = "main";

    public bool completed;
    private bool acceptInput;

    public void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
        acceptInput = true;
        gameObject.SetActive(true);
        completed = false;
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
        }
    }

    private void RotateWheel(float angle)
    {
        wheel.rotation = Quaternion.Euler(0f, 0f, angle);

        //wrapping the number around the wheel logic:
        currentSegment = (currentRotation / segmentAngle) + 1;
        currentSegment = gameManager.mod((currentSegment - 1 + segmentNumber), segmentNumber) + 1;

        number.text = "key: " + Mathf.RoundToInt(currentSegment).ToString(); //update key text

        ciphertext.text = Decipher(originalCiphertext, Mathf.RoundToInt(currentSegment)); //update deciphered text

        if (Mathf.RoundToInt(currentSegment) == correctKey)
        {
            //make it so you can press a button to check if you can proceed. it will allow you to take your time and think over the answer rather that it immediately moving on

            //StartCoroutine(Wait());
            //gameObject.SetActive(false);

            completed = true;
            acceptInput = false;
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
            DialogueManager.GetInstance().JumpToKnot(knotName);
        }
    }

    IEnumerator Wait()//waits so the player has time to see what the correct key and message was. not a good solution - no player control
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
        completed = true;
    }

    //https://www.c-sharpcorner.com/article/caesar-cipher-in-c-sharp/ 17/02/2023
    public static char cipher(char ch, int key)
    {
        if (!char.IsLetter(ch))
        {

            return ch;
        }

        char d = char.IsUpper(ch) ? 'A' : 'a';
        return (char)((((ch + key) - d) % 26) + d);

    }

    public static string Encipher(string input, int key)
    {
        string output = string.Empty;

        foreach (char ch in input)
        {
            output += cipher(ch, key);
        }

        return output;
    }

    public static string Decipher(string input, int key)
    {
        return Encipher(input, 26 - key);
    }
    //--------------------------------------------



}



