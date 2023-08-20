using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PhishingSceneManager : MonoBehaviour
{
    /* this script controls the flow of the phishing puzzles in the soceng (social engineering) scene. on start, it chooses 
    clue variables from variable lists at random, and assigns these variables to the ink scripts of each of the 6 interactable 
    npcs relating to the puzzle. each related npc is hardcoded to jump to a knot named clue1 through to clue6 within the 
    clue file that it is given. when the player interacts with one of the npcs, they say the text that corresponds to their 
    assigned knot in the given file. the clues are slightly different each time the scene is played because of the randomised 
    variables. this adds replay value to the game by making it unpredictable and harder to cheat on by memorising the puzzle 
    answers.
    then the script populates a word grid with correct and incorrect words so the player has to select all the correct words and none of the 
    incorrect words to "write" a phishing email. this shows the importance of needing to do research on the company/
    target before attemptingphishing (which teaches players that successful phishing emails are difficult to recognise 
    because they can be individually tailored to them).

    this script is separate from the minigame script because of the methods and code that need to execute before the puzzle panel is set active (inactive objects cannot be accessed or modified).
    it can be thought of as a scene manager script because it intialises variables within several scene components before the player interacts with them.
    */

    private GameManager gameManager;

    [SerializeField] private GameObject minigamePanel;
    [SerializeField] private GameObject keypadPanel;
    [SerializeField] private GameObject keypadTrigger;
    [SerializeField] private GameObject receptionistTrigger;
    [SerializeField] private GameObject battleTrigger;


    //clue file things
    [SerializeField] private Collider2D[] triggers; //contains the trigger colliders of the interactable robots relating to the puzzle

    //lists
    public List<string> correctWords = new List<string>() { "sticky note", "code", "offices", "lunch" };
    private List<string> incorrectWords = new List<string> { "boss", "password", "elevator", "exit", "buttons" };
    //public List<string> emails; //stores correct emails
    public string email;
    public List<string> tempIncorrectWords = new List<string>();

    //word grid
    public int wordGridColumns = 4;
    public int wordGridRows = 3;
    public List<List<string>> wordGrid = new List<List<string>>(); //using a 2d list instead of jagged array to make it easier to use



    void Start()
    {
        //ink variables
        string generalVariable; //variables to be inserted into the dialogue script
        string singularVariable;
        string currentValue;
        string receptionistName;
        string employeeName;
        int passcode;
        string firstName;
        string lastName;

        List<string> generalVariables = new List<string>() { "day of the week", "month", "hour of the day" };
        List<string> singularVariables = new List<string>() { "day", "month", "hour" };
        List<List<string>> possibleValues = new List<List<string>>(){
        new List<string>(){"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"},
        new List<string>(){"January", "February", "March", "April", "May", "June", "July",
            "August", "September", "October", "November", "December"}
        };
        List<string> maleNames = new List<string>() {
            "James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph", "Thomas", "Charles",
            "Daniel", "Matthew", "Anthony", "Donald", "Mark", "Paul", "Steven", "Andrew", "Kenneth", "George",
            "Joshua", "Kevin", "Brian", "Edward", "Ronald", "Timothy", "Jason", "Jeffrey", "Ryan", "Jacob"
        };

        List<string> femaleNames = new List<string>() {
            "Emma", "Olivia", "Ava", "Isabella", "Sophia", "Mia", "Charlotte", "Amelia", "Harper", "Evelyn",
            "Abigail", "Emily", "Elizabeth", "Sofia", "Avery", "Ella", "Scarlett", "Grace", "Victoria", "Chloe",
            "Lily", "Zoe", "Madison", "Emily", "Aubrey", "Addison", "Hannah", "Aria", "Ella", "Natalie"
        };

        List<string> neutralNames = new List<string>(){
        "Alex", "Cameron", "Casey", "Charlie", "Frankie", "Hayden", "Harper", "Jamie", "Jayden", "Jesse", "Jordan", "Kai", "Kelly", "Morgan", "Pat", "Riley", "Robin", "Rowan", "Ryan", "Sam","Taylor"
        };

        List<string> lastNames = new List<string>() { "Chen", "Gomez", "Kim", "Li", "Martinez", "Patel", "Sato", "Smith", "Tanaka", "Wang", "Nguyen", "Ahmed", "Garcia", "Kumar", "Lopez", "Choi", "Singh", "Inoue", "Rodriguez", "Khan", "Hernandez", "Das", "Brown", "Le", "Liu", "Thompson", "Nakamura", "Gupta", "Yamamoto", "Gonzalez"
        };

        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();

        int index = Random.Range(0, generalVariables.Count);

        generalVariable = generalVariables[index];
        singularVariable = singularVariables[index];

        List<string> hourList = new List<string>();
        for (int i = 0; i < 24; i++)
        {
            //string hourString = i.ToString("D2"); //format the number with leading zeros
            string hourString = i.ToString();
            hourList.Add(hourString + " o'clock");
        }
        possibleValues.Add(hourList);

        int index2 = Random.Range(0, possibleValues[index].Count);

        currentValue = possibleValues[index][index2];

        receptionistName = femaleNames[Random.Range(0, femaleNames.Count)];
        employeeName = maleNames[Random.Range(0, maleNames.Count)];

        foreach (Collider2D trigger in triggers)
        {
            trigger.GetComponent<DialogueTrigger>().variableNames = new List<string>() { "generalVariable", "singularVariable", "currentValue", "receptionistName", "employeeName" };
            trigger.GetComponent<DialogueTrigger>().variableValues = new List<string>() { generalVariable, singularVariable, currentValue, receptionistName, employeeName };
        }

        //generate random number, send it to minigame, send it to keycard

        passcode = Random.Range(1000, 10000);
        minigamePanel.GetComponent<PhishingPuzzle>().variableNames = new List<string>() { "receptionistName", "employeeName", "passcode" };
        minigamePanel.GetComponent<PhishingPuzzle>().variableValues = new List<string>() { receptionistName, employeeName, passcode.ToString() };
        keypadPanel.GetComponent<KeypadController>().correctPin = passcode.ToString();
        keypadTrigger.GetComponent<DialogueTrigger>().variableNames = new List<string>() { "passcode" };
        keypadTrigger.GetComponent<DialogueTrigger>().variableValues = new List<string>() { passcode.ToString() };

        firstName = neutralNames[Random.Range(0, neutralNames.Count)];
        lastName = lastNames[Random.Range(0, lastNames.Count)];

        receptionistTrigger.GetComponent<DialogueTrigger>().variableNames = new List<string>() { "firstName", "lastName" };
        receptionistTrigger.GetComponent<DialogueTrigger>().variableValues = new List<string>() { firstName, lastName };
        battleTrigger.GetComponent<DialogueTrigger>().variableNames = new List<string>() { "firstName", "lastName" };
        battleTrigger.GetComponent<DialogueTrigger>().variableValues = new List<string>() { firstName, lastName };

        correctWords.AddRange(new List<string>() { currentValue, receptionistName, employeeName });
        email = "Hi " + receptionistName + ", \nI have lost my sticky note with today's (" + currentValue + "'s) code to the door on the second floor again! If management finds out that I didn't actually memorise the code but wrote it somewhere that others can see, I will be fired for sure! Could you please send me the code over email before lunch break is over? Management asked me to go to the offices wing after lunch and I don't want to look bad! \nThanks a lot,\n" + employeeName;

        //initialise grid
        for (int row = 0; row < wordGridRows; row++) //was wordGridColumns for some reason??
        {
            wordGrid.Add(new List<string>());
            for (int col = 0; col < wordGridColumns; col++) //was wordGridRows for some reason??
            {
                wordGrid[row].Add("");
            }
        }

        PopulateWordGrid(correctWords);

        //once the grid has been filled with correct words, there will be spaces left to add. the following code randomises the list of incorrect words and takes the number of incorrect words that is enough to fill the gaps in the grid. the randomisation is done because otherwise, the first few words in the list will be used more often than the last few words depending on the size of the grid and the number of correct words in the current clue file

        int maxWordsFromIncorrectWords = wordGridColumns * wordGridRows - correctWords.Count;
        if (maxWordsFromIncorrectWords < incorrectWords.Count)
        {
            tempIncorrectWords = gameManager.ListShuffle(incorrectWords).GetRange(0, maxWordsFromIncorrectWords);
        }
        else
        {
            tempIncorrectWords = incorrectWords;
        }
        PopulateWordGrid(tempIncorrectWords);

        //debugging te contents of the grid to see what's causing an issue
        /*  for (int i = 0; i < wordGrid.Count; i++)
         {
             string rowData = "";
             for (int j = 0; j < wordGrid[i].Count; j++)
             {
                 rowData += wordGrid[i][j] + "\t";
             }
             Debug.Log(rowData);
         } */
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
