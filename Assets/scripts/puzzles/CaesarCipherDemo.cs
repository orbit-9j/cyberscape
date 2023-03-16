using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaesarCipherDemo : MonoBehaviour
{
    /* this script is a demonstration of the caesar cipher. it shows how the alphabet shifts with different key values */

    [SerializeField] private TextMeshProUGUI ciphertextAlphabet;
    [SerializeField] private TextMeshProUGUI keyText;
    private int key = 0;
    private string alphabet = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";
    private int displacement = 0;

    public bool completed;
    private bool acceptInput;

    public void Start()
    {
        gameObject.SetActive(true);
        ciphertextAlphabet.text = alphabet;
    }

    void Update()
    {
        //do i even need negative keys? i should just wrap them since irl -1 would be 25. todo
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            displacement--;
            key--;
            if (displacement < 0)
            {
                displacement += 26;
            }

            if (key < -25)
            {
                key = 0;
            }

            DisplaceAlphabet();
            keyText.text = "key: " + key;
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

            DisplaceAlphabet();
            keyText.text = "key: " + key;
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
}
