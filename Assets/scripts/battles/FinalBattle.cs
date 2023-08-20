using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class FinalBattle : BattleMinigame
{
    [SerializeField] private List<TextMeshProUGUI> cells; // word grid cells
    private List<string> terms = new List<string>(){
        "2 Factor Authentication (2FA)", "malware", "password manager", "software patching", "firewall", "digital footprint"
    };
    private List<string> definitions = new List<string>(){
        "requiring two forms of authentication to access data", "malicious software made to harm your computer or steal your data", "a program that securely stores your account credentials for different accounts", "a software update that fixes security vulnerabilities and bugs", "software that monitors network traffic and decides if it should be blocked or allowed", "traces of your online activity that can be followed"
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
            foreach (TextMeshProUGUI item in sublist)
            {
                item.text = "";
            }
            wordGrid.Add(sublist);
        }

        PopulateGrid();
        selectedTextbox = wordGrid[row][column];
        HighlightText();
    }

    private void PopulateGrid()
    {
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
    }

    void Update()
    {
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

                    //game freezes here
                    if (correctCells.Count == numberOfRows * numberOfColumns)
                    {
                        round++;
                        if (round < numberOfRounds)
                        {
                            foreach (var row in wordGrid)
                            {
                                foreach (var cell in row)
                                {
                                    cell.text = "";
                                }
                            }
                            //Debug.Log("cell content: " + cells[0].text);
                            correctCells.Clear();
                            PopulateGrid();

                            foreach (TextMeshProUGUI cell in cells)
                            {
                                cell.color = Color.white;
                            }

                            selectedTextbox = wordGrid[row][column];
                            HighlightText();
                        }
                        else if (round == numberOfRounds)
                        {
                            winState = true;
                            EndMinigame();
                        }
                    }
                    //don't forget win condition/what damage you do depending on how many rounds idk
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
        foreach (TextMeshProUGUI cell in cells)
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

    private void HighlightText()
    {
        selectedTextbox.color = Color.yellow;
    }
}
