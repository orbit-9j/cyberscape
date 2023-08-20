using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhishingPuzzleController : MonoBehaviour
{
    /* this script controls the flow of the phishing puzzle in the soceng (social engineering) scene. on start, it chooses a clue dialogue 
    file at random, and assigns that file to each of the 6 interactable npcs relating to the puzzle. each related npc is hardcoded 
    to jump to a knot named clue1 through to clue6 within the clue file that it is given. when the player interacts with one of the 
    npcs, they say the text that corresponds to their assigned knot in the given file. this makes clues random each playthrough
    and therefore adds replay value to the game by making it unpredictable and harder to cheat on by memorising the puzzle answers.
    then the script populates a word grid with correct and incorrect words so the player has to select all the correct words and none of the 
    incorrect words to "write" a phishing email. this shows the importance of needing to do research on the company/target before attempting
    phishing.

    this script is separate from the minigame script because of the methods and code that need to execute before the puzzle panel is set active (inactive objects cannot be accessed or modified)
    */

    /* the code worked fine before i moved some stuff to the minigame script and now i need to find why the grid isn't displaying properly
    but unity keeps freezing and i gave up after an hour of trying to get the game to run */

    /* would this actually work better as classes? cuz then i can have a class per puzzle piece, like clue file, correct words list, and email*/

    private GameManager gameManager;

    //clue file things
    [SerializeField] private Collider2D[] triggers; //contains the trigger colliders of the interactable robots relating to the puzzle
    [SerializeField] private TextAsset[] inkJSONList; //contains the files that the robots pull clues from. each file is a set of related clues
    public int clueFileNum; //the number of the chosen clue file

    //lists
    private List<List<string>> correctWords;
    private List<string> incorrectWords;
    public List<string> emails; //stores correct emails
    public List<string> tempCorrectWords;
    public List<string> tempIncorrectWords = new List<string>();

    //word grid
    public int wordGridColumns = 4;
    public int wordGridRows = 3;
    public List<List<string>> wordGrid = new List<List<string>>(); //using a 2d list instead of jagged array to make it easier to use

    [SerializeField] private GameObject minigamePanel;

    void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();

        //why is it only selecting the first file all the time?? - i think it's the inspector panel text field glitch, may need to set it manually for the time being
        int clueFileNum = Random.Range(0, inkJSONList.Length); //randomly select clue file from list of files

        foreach (Collider2D trigger in triggers) //assign the clue file to each related npc
        {
            trigger.GetComponent<DialogueTrigger>().inkJSON = inkJSONList[clueFileNum];
        }

        //initialise grid
        for (int row = 0; row < wordGridRows; row++) //was wordGridColumns for some reason??
        {
            wordGrid.Add(new List<string>());
            for (int col = 0; col < wordGridColumns; col++) //was wordGridRows for some reason??
            {
                wordGrid[row].Add("");
            }
        }

        correctWords = new List<List<string>>();
        correctWords.Add(new List<string> { "Wednesday", "Jen", "sticky note", "code", "Mark", "offices", "lunch" }); //correct words for clue file 1
        correctWords.Add(new List<string> { "test", "test", "test", "test" }); //correct words for clue file 2
        incorrectWords = new List<string> { "boss", "password", "elevator", "exit", "buttons" };
        emails.Add("Hi Jen, \nI have lost my sticky note with today's (Wednesday's) code to the door on the second floor again! If management finds out that I didn't actually memorise the code but wrote it somewhere that others can see, I will be fired for sure! Could you please send me the code over email before lunch break is over? Management asked me to go to the offices wing after lunch and I don't want to look bad! \nThanks a lot,\nMark");
        emails.Add("test");

        //add the correct words relating to the selected clue file to temporary list so that both the incorrect word list and the correct word lists are the same dimension
        tempCorrectWords = new List<string>();
        tempCorrectWords.AddRange(correctWords[clueFileNum]);
        //Debug.Log("final sliced correct word list: " + string.Join(", ", tempCorrectWords));
        Debug.Log("wordGrid columns: " + wordGridColumns.ToString() + ", wordGrid rows: " + wordGridRows.ToString() + ", count of correct words: " + tempCorrectWords.Count.ToString());
        PopulateWordGrid(tempCorrectWords/* , startPosition, elementSize */); //adds correct words to random positions on the grid

        //once the grid has been filled with correct words, there will be spaces left to add. the following code randomises the list of incorrect words and takes the number of incorrect words that is enough to fill the gaps in the grid. the randomisation is done because otherwise, the first few words in the list will be used more often than the last few words depending on the size of the grid and the number of correct words in the current clue file

        //idea: dynamically change grid cell number and size depending on the list of correct words as they're likely to be of different lengths. but it would probably be too overkill
        int maxWordsFromIncorrectWords = wordGridColumns * wordGridRows - tempCorrectWords.Count;
        if (maxWordsFromIncorrectWords < incorrectWords.Count)
        {
            tempIncorrectWords = gameManager.ListShuffle(incorrectWords).GetRange(0, maxWordsFromIncorrectWords);
        }
        else
        {
            tempIncorrectWords = incorrectWords;
        }
        //string joinedList = string.Join(", ", incorrectWords);
        //Debug.Log("final incorrect word list: " + joinedList);
        //Debug.Log("final sliced incorrect word list: " + string.Join(", ", tempIncorrectWords));

        PopulateWordGrid(tempIncorrectWords); //adds incorrect words to fill in the gaps between words in the grid

        //debugging te contents of the grid to see what's causing an issue
        for (int i = 0; i < wordGrid.Count; i++)
        {
            string rowData = "";
            for (int j = 0; j < wordGrid[i].Count; j++)
            {
                rowData += wordGrid[i][j] + "\t";
            }
            Debug.Log(rowData);
        }

        Minigame(); //calling it from here for debug purposes
    }

    private void Minigame()
    {
        minigamePanel.SetActive(true);
    }

    private void PopulateWordGrid(List<string> list) //takes in list of correct or incorrect words
    {
        //keep track of what words have been added to the grid to compare against the current word to be added 9avoids duplicate words on grid)
        List<string> addedWords = new List<string>();
        int numOfAddedWords = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (numOfAddedWords < list.Count) //ends after all words in the list have been added - prevents an endless loop when a list has ran out of words but there are still empty cells in the grid
            {
                //select random grid cell
                int x = Random.Range(0, wordGridRows); //was wordGridColumns for some reason??
                int y = Random.Range(0, wordGridColumns); //was wordGridRows for some reason??
                //Debug.Log("x: " + x.ToString() + ", y: " + y.ToString());

                if (wordGrid[x][y] == "")// Check if cell is empty
                {
                    if (addedWords.Contains(list[i])) //check if this word has already been added
                    {
                        i--; //try again in a different cell 
                    }
                    else //if not, add the word to the cell
                    {
                        wordGrid[x][y] = list[i];
                        addedWords.Add(list[i]);
                        numOfAddedWords++;
                    }
                }
                else
                {
                    i--; // Try again
                }
            }
        }
    }
}
