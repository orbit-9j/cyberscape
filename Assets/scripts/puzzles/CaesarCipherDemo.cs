using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaesarCipherDemo : MonoBehaviour
{
    /* this script is a demonstration of the caesar cipher. it shows how the alphabet shifts with different key values */
    private GameManager gameManager;

    [SerializeField] private TextMeshProUGUI ciphertextAlphabet;
    [SerializeField] private TextMeshProUGUI keyText;
    private int key = 0;
    private string alphabet = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";
    private int displacement = 0;

    private string plaintext = "What kind of sentence?";
    [SerializeField] private TextMeshProUGUI plaintextMessage;
    private string ciphertext = "What kind of sentence?";
    [SerializeField] private TextMeshProUGUI ciphertextMessage;

    public bool completed;
    private bool acceptInput;

    public void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
        gameObject.SetActive(true);
        ciphertextAlphabet.text = alphabet;

        //start off with the key of 0 so the ciphertext and plaintext are the same
        plaintextMessage.text = plaintext;
        ciphertextMessage.text = ciphertext;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            displacement--;
            key--;
            if (displacement < 0)
            {
                displacement += 26;
            }

            if (key < 0)
            {
                key = 26;
            }

            Finalise();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            displacement++;
            key++;
            if (displacement >= 26)
            {
                displacement -= 26;
            }

            if (key > 25)
            {
                key = 0;
            }

            Finalise();
        }
    }

    private void DisplaceAlphabet() // Update the alphabet text with the displaced string
    {
        string[] alphabetArray = alphabet.Split(' ');
        List<string> displacedAlphabet = new List<string>();
        for (int i = 0; i < alphabetArray.Length; i++)
        {
            int index = (i + displacement) % 26;
            displacedAlphabet.Add(alphabetArray[index]);
        }
        ciphertextAlphabet.text = string.Join(" ", displacedAlphabet);
    }

    private void Finalise()
    {
        DisplaceAlphabet();
        keyText.text = "key: " + key;
        ciphertextMessage.text = gameManager.Encipher(plaintext, key);
    }
}
