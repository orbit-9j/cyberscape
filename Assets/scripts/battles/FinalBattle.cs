using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class FinalBattle : BattleMinigame
{
    [SerializeField] private List<TextMeshProUGUI> cells; // word grid cells
    private List<string> terms = new List<string>(){
        "2 Factor Authentication (2FA)", "malware", "password manager", "software patching", "firewall", "digital footprint", "anti-virus software", "asymmetric cryptography", "audit log", "authentication", "backup", "biometric", "cybersecurity", "Denial Of Service (DoS) attack", "hacker", "internet", "keylogger", "network", "password", "personal data", "privacy", "remote access", "trojan horse"
    };
    private List<string> definitions = new List<string>(){
        "requiring two forms of authentication to access data", "malicious software made to harm your computer or steal your data", "a program that securely stores your account credentials for different accounts", "a software update that fixes security vulnerabilities and bugs", "software that monitors network traffic and decides if it should be blocked or allowed", "traces of your online activity that can be followed", "a program that detects potential malware on your computer", "uses an encryption algorithm where the encryption and decryption keys are different", "a record of user activity on a computer system", "verifying the identity of a user trying to access a system", "a copy of data on a computer system kept to restore in case of a system failure", "a body measurement that can be used for personal identification", "the protection of information stored in or processed by computer systems from misuse", "cyber attack that overwhelms a computer system's network connections and makes it inaccessible", "a person who gains access to a computer system with often malicious intent", "a global network of computers that enables the sharing of information across a wide geographical area", "malware that spys on a user's keyboard input and sends it to the attacker", "a collection of computers that communicate with each other through wired or wireless connections", "a sequence of characters used to gain access to a computer system", "information that can personally identify you", "hiding data from unauthorised access", "gaining access to information on a computer system that is not on the same network as you", "malware that poses as other legitimate software"
    };

    private List<List<TextMeshProUGUI>> wordGrid = new List<List<TextMeshProUGUI>>();

    private int numberOfRounds = 1;
    private int round = 0;

    private int column = 0;
    private int row = 0;

    private int numberOfColumns = 2;
    private int numberOfRows = 4;

    private List<TextMeshProUGUI> selectedCells = new List<TextMeshProUGUI>();

    private TextMeshProUGUI selectedTextbox;
    private List<TextMeshProUGUI> correctCells = new List<TextMeshProUGUI>();

    void Start()
    {
        //initialise word grid
        for (int i = 0; i < cells.Count; i += numberOfColumns)
        {
            List<TextMeshProUGUI> sublist = cells.GetRange(i, Math.Min(numberOfColumns, cells.Count - i));
            wordGrid.Add(sublist);
        }

        ResetMinigame();
    }

    private void PopulateGrid()
    {
        selectedCells.Clear();
        correctCells.Clear();

        foreach (List<TextMeshProUGUI> row in wordGrid)
        {
            foreach (TextMeshProUGUI cell in row)
            {
                cell.text = "";
            }
        }

        List<string> added = new List<string>();
        int numberOfAddedTerms = 0;
        //populate grid with terms

        while (numberOfAddedTerms < numberOfRows)
        {
            int randomColumn = UnityEngine.Random.Range(0, numberOfColumns);
            int randomRow = UnityEngine.Random.Range(0, numberOfRows);
            string tempTerm = terms[UnityEngine.Random.Range(0, terms.Count)];

            if (!added.Contains(tempTerm) && wordGrid[randomRow][randomColumn].text == "") //if term hasn't been added and the grid cell is empty
            {
                wordGrid[randomRow][randomColumn].text = tempTerm;
                added.Add(tempTerm);
                numberOfAddedTerms++;
            }
        }

        //now do it for matching definitions
        numberOfAddedTerms = 0;
        while (numberOfAddedTerms < numberOfRows)
        {
            int randomColumn = UnityEngine.Random.Range(0, numberOfColumns);
            int randomRow = UnityEngine.Random.Range(0, numberOfRows);
            int index = terms.IndexOf(added[numberOfAddedTerms]);
            string matchingDefinition = definitions[index];

            if (wordGrid[randomRow][randomColumn].text == "")
            {
                wordGrid[randomRow][randomColumn].text = matchingDefinition;
                numberOfAddedTerms++;
            }
        }

        //reset colours
        foreach (List<TextMeshProUGUI> row in wordGrid)
        {
            foreach (TextMeshProUGUI cell in row)
            {
                cell.color = Color.white;
            }
        }
    }

    public override void ResetMinigame()
    {
        round = 0;
        PopulateGrid();

        selectedTextbox = wordGrid[row][column];
        HighlightText();
    }

    void Update()
    {
        base.Update();
        //same grid navigation as phishing puzzle
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

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (selectedCells.Count == 2)
                {
                    selectedCells.Clear();
                }

                selectedCells.Add(selectedTextbox);

                if (selectedCells.Count == 2) //check if the selected cells are matching terms and definitions
                {
                    //first find if it's a term or definition pair
                    if ((terms.Contains(selectedCells[0].text) && definitions.Contains(selectedCells[1].text)) || (terms.Contains(selectedCells[1].text) && definitions.Contains(selectedCells[0].text)))
                    { //if the selected textboxes contain 1 term and 1 definition, proceed to next step
                        if (terms.Contains(selectedCells[0].text))
                        {
                            int index = terms.IndexOf(selectedCells[0].text);
                            //string term = selectedCells[0].text;
                            if (selectedCells[1].text == definitions[index])
                            { //if definition matches the term, make them all green and add them to the list of correct cells
                                correctCells.AddRange(selectedCells);
                                selectedCells[0].color = Color.green;
                                selectedCells[1].color = Color.green;
                            }
                        }
                        else if (terms.Contains(selectedCells[1].text))
                        {
                            int index = terms.IndexOf(selectedCells[1].text);
                            if (selectedCells[0].text == definitions[index])
                            {
                                correctCells.AddRange(selectedCells);
                                selectedCells[0].color = Color.green;
                                selectedCells[1].color = Color.green;
                            }
                        }
                    }

                    selectedCells.Clear();

                    if (correctCells.Count == numberOfRows * numberOfColumns)
                    {
                        round++;
                        if (round < numberOfRounds)
                        {
                            foreach (List<TextMeshProUGUI> row in wordGrid)
                            {
                                foreach (TextMeshProUGUI cell in row)
                                {
                                    cell.text = "";
                                }
                            }
                            //Debug.Log("cell content: " + cells[0].text);
                            PopulateGrid();

                            HandleSelection();
                        }
                        else if (round == numberOfRounds)
                        {
                            winState = true;
                            EndMinigame();
                        }
                    }
                }
            }
        }
    }

    private void HandleSelection()
    {
        DeselectText();
        selectedTextbox = wordGrid[row][column];
        HighlightText();
    }

    private void DeselectText()
    {
        foreach (List<TextMeshProUGUI> row in wordGrid)
        {
            foreach (TextMeshProUGUI cell in row)
            {
                if (selectedCells.Contains(cell))
                {
                    cell.color = Color.grey;
                }
                else if (correctCells.Contains(cell))
                {
                    cell.color = Color.green;
                }
                else
                {
                    cell.color = Color.white;
                }
            }
        }
    }

    private void HighlightText()
    {
        selectedTextbox.color = Color.yellow;
    }
}
