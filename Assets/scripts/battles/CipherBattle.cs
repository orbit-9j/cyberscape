using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CipherBattle : BattleMinigame
{

    //very similar code to the caesar cipher demo but since i can only inherit code from one class i couldn't inherit from it


    [SerializeField] private TextMeshProUGUI ciphertextAlphabet;
    [SerializeField] private TextMeshProUGUI plaintextAlphabet;
    private string alphabet = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";
    private int displacement = 0;

    private string plaintext = "";
    [SerializeField] private TextMeshProUGUI plaintextMessage;
    string plaintextStr;
    private string ciphertext = "";
    [SerializeField] private TextMeshProUGUI ciphertextMessage;
    string ciphertextStr;
    private bool encrypt = false;
    private List<string> commands = new List<string>(){
        "delete System32", "shutdown /s", "rm -rf /"
    };

    private List<string> numbers = new List<string>()
    {
        "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten","eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen", "twenty", "twenty-one", "twenty-two", "twenty-three","twenty-four", "twenty-five"
    };

    [SerializeField] private TextMeshProUGUI keyText;
    private int key;
    private int nextKey;
    private int userInputKey;

    private int numberOfRounds = 3;
    private int currentRound = 1;
    //bool solved = false;


    [SerializeField] private TextMeshProUGUI type;


    void Start()
    {
        base.Start();

        ciphertextAlphabet.text = alphabet;
        plaintextMessage.text = "plaintext:";
        ciphertextMessage.text = "ciphertext:";

        key = Random.Range(1, 26);
        Decrypt();
    }

    void Update() //accepts and processes input
    {//somehow differentiate between which direction to go in and how to register that difference
     // encrypt = key, decrypt = mod(26,key)?
        base.Update();
        if (acceptInput)
        {
            //take turns changing which alphabet moves
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                displacement--;
                userInputKey--;
                if (displacement < 0)
                {
                    displacement += 26;
                }

                if (userInputKey < 0)
                {
                    userInputKey = 26;
                }

                Finalise();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                displacement++;
                userInputKey++;
                if (displacement >= 26)
                {
                    displacement -= 26;
                }

                if (userInputKey > 25)
                {
                    userInputKey = 0;
                }

                Finalise();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (!encrypt && plaintext == ciphertextStr)
                {
                    Encrypt();
                }
                else if (!encrypt && plaintext != ciphertextStr)
                {
                    gameManager.FlashText(plaintextMessage, Color.white, Color.red);
                }

                else if (encrypt && userInputKey == key)
                {
                    Decrypt();
                }
                else if (encrypt && userInputKey != key)
                {
                    gameManager.FlashText(ciphertextMessage, Color.white, Color.red);
                }
            }
        }
    }

    private void Encrypt()
    {
        encrypt = true;
        type.text = "Encrypt!";

        key = nextKey;

        plaintextStr = ">" + commands[Random.Range(0, commands.Count)];
        plaintextMessage.text = plaintextStr;
        plaintext = plaintextStr;
        //plaintext = gameManager.Decipher(plaintextStr, key);

        ciphertext = gameManager.Encipher(plaintextStr, userInputKey);
        ciphertextMessage.text = ciphertext;
    }

    private void Decrypt()
    {
        if (currentRound <= numberOfRounds)
        {
            encrypt = false;
            type.text = "Decrypt!";

            userInputKey = 0;
            //keyText = "key: " + userInputKey;

            nextKey = Random.Range(1, 26);
            ciphertextStr = "the next key is " + numbers[nextKey - 1];
            ciphertext = gameManager.Encipher(ciphertextStr, key);
            ciphertextMessage.text = ciphertext;

            plaintext = gameManager.Decipher(ciphertext, userInputKey);
            plaintextMessage.text = plaintext;

            currentRound++;
        }
        else
        {
            winState = true; //all rounds are completed before time's up = win
            EndMinigame();
        }

    }

    private void DisplaceAlphabet() // Update the alphabet text with the displaced string
    {
        //decide which alphabet to move

        string[] alphabetArray = alphabet.Split(' ');
        List<string> displacedAlphabet = new List<string>();
        for (int i = 0; i < alphabetArray.Length; i++)
        {
            int index = (i + displacement) % 26; //should it be the mod function???
            displacedAlphabet.Add(alphabetArray[index]);
        }

        if (encrypt)
        {
            ciphertextAlphabet.text = string.Join(" ", displacedAlphabet);
        }
        else
        {
            plaintextAlphabet.text = string.Join(" ", displacedAlphabet);
        }
    }

    private void Finalise()
    {
        DisplaceAlphabet();
        keyText.text = "key: " + userInputKey;

        if (encrypt)
        {
            ciphertext = gameManager.Encipher(plaintext, userInputKey);
            ciphertextMessage.text = ciphertext;
        }
        else
        {
            int tempKey = gameManager.modInt(26 - userInputKey, 26);
            plaintext = gameManager.Decipher(ciphertext, tempKey);
            plaintextMessage.text = plaintext;
        }
    }
}
