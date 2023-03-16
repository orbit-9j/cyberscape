using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
//using UnityEngine.UI; //imported to change choices panel colour

//https://www.youtube.com/watch?v=vY0Sk93YUhA 12/02/2023
//https://www.youtube.com/watch?v=tVrxeUIEV9E 13/02/2023
//https://www.youtube.com/watch?v=2I92egFvC80 02/03/2023
//refer to github code that the videos link
public class DialogueManager : MonoBehaviour
{
    /* this script manages showing the dialogue, showing choices, starting and ending a puzzle/minigame, displaying character names and portraits.
    i have also added the functionality to be able to jump to a certain knot in the dialogue file, eg after a puzzle is completed and requires 
    dialogue afterwards, to add more flexibility to how lessons are taught and how puzzles are integrated */

    //singleton class 
    private static DialogueManager instance;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    [SerializeField] private float typingSpeed = 0.03f;
    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    //tags
    private Animator layoutAnimator;
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private const string PUZZLE_TAG = "puzzle"; //tag to indicate puzzle start
    private Coroutine displayLineCoroutine;
    private const string layoutYou = "right";
    private const string layoutThem = "left";

    [Header("Puzzles")] //references to the puzzles to be played
    //it would be a good idea to make the puzzles derive from a parent puzzle class. it could have the puzzleIsPlaying variable and set panel to active on start etc to make the puzzle scripts less redundant
    [SerializeField] private GameObject bruteForcePuzzle;
    [SerializeField] private GameObject caesarCipherDemoPuzzle;
    [SerializeField] private GameObject battlePanel;
    public bool puzzlePlaying;
    private GameObject currentPuzzle;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("more than one dialogue manager on the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        layoutAnimator = dialoguePanel.GetComponent<Animator>();

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        displayNameText.text = "???";
        portraitAnimator.Play("default");
        layoutAnimator.Play(layoutThem);
        //choicesPanel.SetActive(false); //added to change overlay

        ContinueStory();
        //dialogueText.text = currentStory.currentText;
    }

    private void Update()
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        //if (InputManager.GetInstance().GetSubmitPressed())
        if (currentStory.currentChoices.Count == 0 && Input.GetKeyDown(KeyCode.Space))
        {
            ContinueStory();
        }
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //dialogueText.text = currentStory.Continue();
            if (displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));

            DisplayChoices();
            HandleTags(currentStory.currentTags);
        }
        else
        {
            ExitDialogueMode(); //changed because i'm not using the input system for this
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(":");
            if (splitTag.Length != 2)
            {
                Debug.LogError("tag parsing problem: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    // compare what tag variable is used and play the corresponding layout animation
                    if (tagValue == "layout_them")
                    {
                        layoutAnimator.Play(layoutThem);
                    }
                    else if (tagValue == "layout_you")
                    {
                        layoutAnimator.Play(layoutYou);
                    }
                    break;
                case PUZZLE_TAG: //controls when puzzles start and end
                    if (tagValue == "bruteForcePuzzle")
                    {
                        currentPuzzle = bruteForcePuzzle;
                        StartCoroutine(WaitForPuzzle());
                    }
                    else if (tagValue == "caesarCipherDemo")
                    {
                        currentPuzzle = caesarCipherDemoPuzzle;
                        caesarCipherDemoPuzzle.GetComponent<CaesarCipherDemo>().Start();
                    }
                    else if (tagValue == "battle")
                    {
                        currentPuzzle = battlePanel;
                        battlePanel.GetComponent<BattleController>().Start();
                    }
                    if (tagValue == "closePuzzle")
                    {
                        currentPuzzle.SetActive(false);
                    }
                    break;
                default:
                    Debug.Log("tag isn't currently being handled: " + tag);
                    break;
            }
        }
    }

    private void DisplayChoices()
    {
        //choicesPanel.GetComponent<Image>().color.a = 0f;
        //choicesPanel.SetActive(true); //added to change overlay
        //choicesPanel.GetComponent<Image>().SetActive(true);//added to change overlay
        List<Choice> currentChoices = currentStory.currentChoices;

        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("too many choices, not enough space...");
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        InputManager.GetInstance().RegisterSubmitPressed();
        //choicesPanel.SetActive(false); //added to change overlay
        ContinueStory();
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    //----------------------
    //own code:

    public void JumpToKnot(string knotName) //adds capability for te dialogue manager to start dialogue at a certain knot in the file, since each scene has one dialogue file for organisation purposes
    {
        currentStory.ChoosePathString(knotName);
        ContinueStory();
    }

    private IEnumerator WaitForPuzzle() //waits for te brute force puzzle to complete. if other puzzles require this function, i will rewrite it to pass the puzzle as a parameter into the function
    {
        puzzlePlaying = true;
        bruteForcePuzzle.GetComponent<BruteForcePuzzleController>().Start();
        yield return new WaitUntil(() => bruteForcePuzzle.GetComponent<BruteForcePuzzleController>().completed);
        puzzlePlaying = false;
    }
}
