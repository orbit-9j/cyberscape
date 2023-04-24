using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject player;

    public bool playerMoves;

    public string playerName = "You";
    public string consoleName = "Console";


    void Awake()
    {
        Instance = this;
        playerMoves = true;
        DontDestroyOnLoad(gameObject);
    }

    //some methods that are used in more than one script

    //c# modulo (%) doesn't work like the modulo i'm used to so i had to find code that does it properly
    //https://stackoverflow.com/questions/1082917/mod-of-negative-number-is-melting-my-brain 17/02/2023
    public float mod(float x, float m)
    {
        return (x % m + m) % m;
    }

    public int modInt(int x, int m)
    {
        return (x % m + m) % m;
    }

    public List<string> ListShuffle(List<string> list)
    {
        //Fisher-Yates shuffle to randomise the words in the incorrect word array, before taking the first n words for the puzzle
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            string value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }

    public TextMeshProUGUI InstantiateText(GameObject screenObject, GameObject textPrefab, string text)
    {
        RectTransform screen = screenObject.GetComponent<RectTransform>();
        float screenWidth = screen.rect.width;
        float screenHeight = screen.rect.height;
        TextMeshProUGUI textObj = Instantiate(textPrefab.GetComponent<TextMeshProUGUI>(), screenObject.transform);
        textObj.rectTransform.sizeDelta = new Vector2(screenWidth, screenHeight);
        textObj.text = text;
        textObj.rectTransform.localPosition = new Vector3(0f, 0f, 0f);
        textObj.alignment = TextAlignmentOptions.Left;
        textObj.enableWordWrapping = true;

        return textObj;
    }

    public void FitToSize(GameObject screenObject, TextMeshProUGUI textObj)
    {
        float fontSize = 200f;
        textObj.fontSize = fontSize;
        textObj.overflowMode = TextOverflowModes.Truncate;
        float preferredHeight = textObj.preferredHeight;
        RectTransform screen = screenObject.GetComponent<RectTransform>();
        float screenHeight = screen.rect.height;
        while (preferredHeight > screenHeight)
        {
            fontSize--;
            textObj.fontSize = fontSize;
            preferredHeight = textObj.preferredHeight;
        }
    }

    public void PressButton(Button buttonObj, Sprite defaultSprite, Sprite pressedSprite)
    {
        buttonObj.image.sprite = pressedSprite;
    }

    public void UnpressButton(Button buttonObj, Sprite defaultSprite)
    {
        float buttonPressDuration = 0.1f; // duration in seconds for which the button should be "pressed"
        StartCoroutine(ResetButtonSprite(buttonPressDuration, buttonObj, defaultSprite));
    }

    public IEnumerator ResetButtonSprite(float duration, Button buttonObj, Sprite defaultSprite)
    {
        yield return new WaitForSeconds(duration);
        buttonObj.image.sprite = defaultSprite;
    }

    public void PressUnpressButton(Button buttonObj, Sprite defaultSprite, Sprite pressedSprite)
    {
        buttonObj.image.sprite = pressedSprite;
        StartCoroutine(ResetButtonSprite(0.1f, buttonObj, defaultSprite));
    }

    public IEnumerator FlashText(TextMeshProUGUI textObj, Color originalColour, Color newColour)
    {
        textObj.color = newColour;
        yield return new WaitForSeconds(0.7f);
        textObj.color = originalColour;
    }

    //https://www.c-sharpcorner.com/article/caesar-cipher-in-c-sharp/ 17/02/2023
    public char cipher(char ch, int key)
    {
        if (!char.IsLetter(ch))
        {

            return ch;
        }

        char d = char.IsUpper(ch) ? 'A' : 'a';
        return (char)((((ch + key) - d) % 26) + d);

    }

    public string Encipher(string input, int key)
    {
        string output = string.Empty;

        foreach (char ch in input)
        {
            output += cipher(ch, key);
        }

        return output;
    }

    public string Decipher(string input, int key)
    {
        return Encipher(input, 26 - key);
    }
    //--------------------------------------------

    //https://www.youtube.com/watch?v=ii31ObaAaJo 12/02/2023
    //public SaveManager saveManager;

    /* void Start()
    {
        saveManager.Load();
    } */

    //button save/load
    /* public void ClickSave()
    {
        saveManager.Save();
    } */

    //key save/load
    //https://www.youtube.com/watch?v=6uMFEM-napE 12/02/2023
    /* private void Update()
    {
        saveManager.data.position_x = player.transform.position.x;
        saveManager.data.position_y = player.transform.position.y;

        if (Input.GetKeyDown(KeyCode.G))
        {
            saveManager.Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            saveManager.Load();
        }
    } */

}
