using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

/* this minigame displays a grid of words that may or may not be relevant to the person/situation that the player needs to research in 
order to write a convincing phishing email. the player is required to select words that relate to the clues they read when they 
interacted with the NPCs on the scene. they have to select all the correct words and none of the incorrect words. if the minigame is
completed successfully, there will be a phishing email displayed on the screen containing the correct words, and a dialogue box will 
show to explain what makes this phishing email effective 

after the player unlocks the email, they will receive a response with the door code, which they will then be able to type into the keypad screen and unlock the door leading to the next level. the keypad functionality is mostly done but still needs to be hooked up to the door */

//add a timer
//launch minigame from trigger on info robot rather than from the puzzle controller

public class PhishingPuzzleMController : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private PhishingPuzzleController script; //reference to the script that sets the stage for the minigame
    public GameObject screenObject; //screen on which the words will be displayed

    //grid variables
    private List<List<string>> wordGrid;
    private int row = 0;
    private int column = 0;
    private int numberOfRows;
    private int numberOfColumns;
    private float spacing = 1.0f;
    public GameObject textPrefab;

    //counters
    private int totalCorrectWords;
    private int chosenCorrectWords;
    [SerializeField] private TextMeshProUGUI correctWordsCounterText;
    private int totalIncorrectWords;
    private int chosenIncorrectWords;
    [SerializeField] private TextMeshProUGUI incorrectWordsCounterText;

    //lists
    private string selectedText; //text of the textbox currently selected by the player
    private TextMeshProUGUI selectedTexbox; //textbox currently selected by player
    private TextMeshProUGUI[] textboxList; //array because the number of components in it won't change
    private List<string> correctWords; //list of correct answers for the minigame, imported from dialogue file
    private List<string> incorrectWords;
    private List<string> chosenWords = new List<string>(); //list of player's answers to the minigame
    private string email;

    //dialogue explaining the puzzle
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private string knotName;

    private bool acceptInput = true;

    void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();

        //initialise counters
        correctWords = script.tempCorrectWords;
        incorrectWords = script.tempIncorrectWords;
        totalCorrectWords = correctWords.Count;
        totalIncorrectWords = incorrectWords.Count;
        correctWordsCounterText.text = "correct words: 0/" + totalCorrectWords;
        incorrectWordsCounterText.text = "incorrect words: 0/" + totalIncorrectWords;

        email = script.emails[script.clueFileNum];
        wordGrid = script.wordGrid;

        /* numberOfColumns = wordGrid.Count;
        numberOfRows = wordGrid[0].Count; */

        /* numberOfRows = wordGrid.Count;
        numberOfColumns = wordGrid[0].Count; */

        numberOfRows = script.wordGridRows;
        numberOfColumns = script.wordGridColumns;

        //calculate the size of word cells in the grid
        RectTransform screen = screenObject.GetComponent<RectTransform>();
        float elementSize = Mathf.Max(screen.rect.width / numberOfColumns, screen.rect.height / numberOfRows) - spacing;
        Vector3 startPosition = new Vector3(-screen.rect.width / 2 + elementSize / 2, screen.rect.height / 2 - elementSize / 2, 0.0f);

        //display the word grid computed in the other script
        for (int r = 0; r < numberOfRows; r++)
        {
            for (int c = 0; c < numberOfColumns; c++)
            {
                Vector3 position = startPosition + new Vector3(c * (elementSize + spacing), -r * (elementSize + spacing), 0.0f);
                GameObject textObj = Instantiate(textPrefab, transform);
                textObj.GetComponent<TextMeshProUGUI>().text = wordGrid[r][c];
                textObj.transform.localPosition = position;
            }
        }

        //Debug.Log("number of columns: " + numberOfColumns.ToString() + ", number of rows: " + numberOfRows.ToString());
        selectedText = wordGrid[row][column];
        Debug.Log("selected text: " + selectedText);
        textboxList = gameObject.GetComponentsInChildren<TextMeshProUGUI>().Where(child => child.gameObject.name == "text(Clone)").ToArray();
        /* foreach (TextMeshProUGUI text in textboxList)
        {
            Debug.Log(text.text);
        } */
        SelectTextbox();
        HighlightText();
    }

    void Update()
    {
        if (acceptInput)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                //column++;
                column = gameManager.modInt((column + 1), numberOfColumns);
                HandleSelection();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                //column--;
                column = gameManager.modInt((column - 1 + numberOfColumns), numberOfColumns);
                HandleSelection();
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                //row--;
                row = gameManager.modInt((row - 1 + numberOfRows), numberOfRows);
                HandleSelection();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                //row++;
                row = gameManager.modInt((row + 1), numberOfRows);
                HandleSelection();
            }

            //weird fix?
            /* if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                column = gameManager.modInt((column + 1), numberOfColumns); //column++;
                HandleSelection();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                column = gameManager.modInt((column - 1 + numberOfColumns), numberOfColumns); //column--;
                HandleSelection();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                row = gameManager.modInt((row - 1 + numberOfRows), numberOfRows); //row--;
                HandleSelection();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                row = gameManager.modInt((row + 1), numberOfRows); //row++;
                HandleSelection();
            } */

            //sometimes unity stops being able to understand the controls despite me not changing their code at all, so i have to rewrite them in a way that makes no sense to me but makes sense to unity. go figure.
            /*  if (Input.GetKeyDown(KeyCode.DownArrow))
             {
                 column = gameManager.modInt((column + 1), numberOfRows); //column++;
                 HandleSelection();
             }
             else if (Input.GetKeyDown(KeyCode.UpArrow))
             {
                 column = gameManager.modInt((column - 1 + numberOfRows), numberOfRows); //column--;
                 HandleSelection();
             }

             if (Input.GetKeyDown(KeyCode.LeftArrow))
             {
                 row = gameManager.modInt((row - 1 + numberOfColumns), numberOfColumns); //row--;
                 HandleSelection();
             }
             else if (Input.GetKeyDown(KeyCode.RightArrow))
             {
                 row = gameManager.modInt((row + 1), numberOfColumns); //row++;
                 HandleSelection();
             }
  */
            if (Input.GetKeyDown(KeyCode.Q))
            {
                AddOrRemoveWord();
            }

            if (Input.GetKeyDown(KeyCode.Return)) //change input system submit
            {
                CheckAnswers();
            }
        }
    }

    private void HandleSelection()
    {
        DeselectText();
        selectedText = wordGrid[row][column];
        SelectTextbox();
        HighlightText();
    }

    private void DeselectText()
    {
        if (chosenWords.Contains(selectedText))
        {
            selectedTexbox.color = Color.grey;
        }
        else
        {
            selectedTexbox.color = Color.white;
        }
    }

    private void SelectTextbox()
    {
        for (int i = 0; i < textboxList.Length; i++)
        {
            if (textboxList[i].text == selectedText)
            {
                selectedTexbox = textboxList[i];
                Debug.Log("selected textbox: " + selectedTexbox.text);
            }
        }
    }

    private void HighlightText()
    {
        selectedTexbox.color = Color.yellow;
    }

    private void AddOrRemoveWord()
    {
        if (chosenWords.Contains(selectedText))
        {
            chosenWords.Remove(selectedText);
            selectedTexbox.color = Color.white;
        }
        else
        {
            chosenWords.Add(selectedText);
            selectedTexbox.color = Color.grey;
        }
    }

    private void CheckAnswers()
    {
        //loop through chosen list and increment the counter for a word that appears in each list (correct and incorrect)
        chosenCorrectWords = 0;
        chosenIncorrectWords = 0;

        foreach (string word in chosenWords)
        {
            if (correctWords.Contains(word))
            {
                chosenCorrectWords++;
            }
            else if (incorrectWords.Contains(word))
            {
                chosenIncorrectWords++;
            }
        }

        correctWordsCounterText.text = "correct words:" + chosenCorrectWords + "/" + totalCorrectWords;
        incorrectWordsCounterText.text = "incorrect words:" + chosenIncorrectWords + "/" + totalIncorrectWords;

        if (chosenCorrectWords == totalCorrectWords && chosenIncorrectWords == 0)
        {
            acceptInput = false;
            StartCoroutine(AfterFlashing()); //flashes text and proceeds to the next stage of the puzzle. a coroutine is required for it to ensure the text stops flashing before it is deleted
        }
        else
        {
            StartCoroutine(FlashText(Color.red)); //flash text grid red for visual feedback that the user input was wrong
        }
    }

    IEnumerator FlashText(Color colour) //briefly change the text grid colour depending on whether the user chose the correct or incorrect words
    {
        foreach (TextMeshProUGUI textPrefab in textboxList)
        {
            textPrefab.color = colour;
        }

        yield return new WaitForSeconds(0.7f);

        foreach (TextMeshProUGUI textPrefab in textboxList)
        {
            if (chosenWords.Contains(textPrefab.text))
            {
                textPrefab.color = Color.grey;
                if (selectedTexbox == textPrefab)
                {
                    textPrefab.color = Color.yellow;
                }
            }
            else
            {
                textPrefab.color = Color.white;
            }
        }
    }

    IEnumerator AfterFlashing()
    {
        yield return StartCoroutine(FlashText(Color.green)); //flash text grid green for visual feedback that the user input was correct

        foreach (TextMeshProUGUI text in textboxList) //delete all text prefabs from the screen
        {
            Destroy(text.gameObject);
        }

        TextMeshProUGUI emailObj = gameManager.InstantiateText(screenObject, textPrefab, email);
        gameManager.FitToSize(screenObject, emailObj);

        yield return new WaitForSeconds(0.5f);

        //add dialogue explaining the "generated" email
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        DialogueManager.GetInstance().JumpToKnot(knotName);
    }
}