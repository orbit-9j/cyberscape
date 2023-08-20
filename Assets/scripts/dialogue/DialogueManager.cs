using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

    private GameManager gameManager;

    [SerializeField] private List<GameObject> puzzles; //list of the puzzles to be played in the scene


    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    [SerializeField] private float typingSpeed = 0.03f;
    public Story currentStory;
    public bool dialogueIsPlaying { get; private set; }

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    //tags
    private Animator layoutAnimator;
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string PUZZLE_TAG = "puzzle"; //tag to indicate puzzle start
    private const string ENDGAME_TAG = "endgame";
    private Coroutine displayLineCoroutine;
    private const string layoutYou = "right";
    private const string layoutThem = "left";

    private GameObject currentPuzzle;

    public string currentKnotName = "";



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
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        layoutAnimator = dialoguePanel.GetComponent<Animator>();

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            choicesText[index].color = Color.black;
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

        /*  if (Input.GetKeyDown(KeyCode.LeftArrow))
         {
             //show previous line (idk if ink has that functionality)
         } */

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentPuzzle != null)
            {
                currentPuzzle.SetActive(false);
                gameManager.playerMoves = true;
            }
            ExitDialogueMode();
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
                    if (tagValue.ToLower() == "you")
                    {
                        displayNameText.text = gameManager.playerName;
                        layoutAnimator.Play(layoutYou);
                    }
                    else if (tagValue.ToLower() == "console")
                    {
                        displayNameText.text = gameManager.consoleName;
                        layoutAnimator.Play(layoutThem);
                    }
                    else
                    {
                        displayNameText.text = tagValue;
                        layoutAnimator.Play(layoutThem);
                    }
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case PUZZLE_TAG: //controls when puzzles start and end
                    PlayPuzzle(tagValue);
                    if (tagValue == "closePuzzle")
                    {
                        currentPuzzle.SetActive(false);
                    }
                    break;

                case ENDGAME_TAG:
                    SceneManager.LoadScene(0, LoadSceneMode.Single);
                    break;
                default:
                    Debug.Log("tag isn't currently being handled: " + tag);
                    break;
            }
        }
    }

    private void DisplayChoices()
    {
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
            choicesText[index].color = Color.white;
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
        //ChoiceColour(0);
    }

    public void MakeChoice(int choiceIndex)
    {
        //ChoiceColour(choiceIndex);
        currentStory.ChooseChoiceIndex(choiceIndex);
        InputManager.GetInstance().RegisterSubmitPressed();
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

    /*   private void ChoiceColour(int choiceIndex)
      {
          foreach (GameObject choice in choices)
          {
              choice.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
          }
          choices[choiceIndex].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;
      } */

    public void JumpToKnot(string knotName) //adds capability for te dialogue manager to start dialogue at a certain knot in the file, since each scene has one dialogue file for organisation purposes
    {
        currentStory.ChoosePathString(knotName);
        currentKnotName = knotName;

        ContinueStory();
    }

    public void JumpToKnot(string knotName, List<string> variableNames, List<string> variableValues)
    {
        currentStory.ChoosePathString(knotName);
        currentKnotName = knotName;

        for (int i = 0; i < variableNames.Count; i++)
        {
            currentStory.variablesState[variableNames[i]] = variableValues[i];
        }

        ContinueStory();
    }


    private void PlayPuzzle(string tagValue)
    {
        if (puzzles != null)
        {
            foreach (GameObject puzzle in puzzles)
            {
                if (puzzle.name == tagValue)
                {
                    currentPuzzle = puzzle;
                    puzzle.SetActive(true);
                    if (tagValue == "battle")
                    {
                        puzzle.GetComponent<BattleManager>().Start(); //resets battle panel in case the player loses the battle and has to do it again
                    }
                }
            }
        }
    }
}
