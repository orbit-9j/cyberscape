using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SocengBattle : BattleMinigame
{
    /* the plan:
    
    attack: 
        the player has to send phishing emails
        
    defend:
        the player has to identify phishing emails */

    private List<string> legitEmails = new List<string>();
    private List<string> phishingEmails = new List<string>();
    private List<string> mixedEmails = new List<string>();
    private int totalEmailsToShow;
    private int emailIndex;
    private string currentEmail;
    private TextMeshProUGUI emailObj;
    private List<int> emailIndexes = new List<int>();
    private List<bool> answerList; //true = phishing, false = legit
    private List<bool> correctAnswerList;

    [SerializeField] private Button answerButton;
    [SerializeField] private GameObject counterText;

    Color32 legitColour = new Color32(180, 120, 0, 255); //yellow-orange
    Color32 phishingColour = new Color32(113, 89, 133, 255); //ashy purple

    //text to display score (how many emails you got right)
    //ideally i'd need to show which ones were wrong and right
    //make a method for the end of the minigame that iterates through the mixed emails arrays, and compares the values for each index between answer list and correct answer list and then flash the text red/green 
    //move the text flash method to game manager and use that

    void Start()
    {
        base.Start();

        answerButton.enabled = true;

        totalEmailsToShow = 3;

        legitEmails.Add("from: sharkhubbot@sharkhub.com \nto:{name}@gmail.com \nDear {name}, \nWe have noticed that your SharkHub account has been accessed from a new location. If this wasn't you, please ensure you are logged in and click the link below to manage your devices: sharkhub.com/profile/mydevices. \nKeep Swimming! \nSharkHub security bot");
        legitEmails.Add("legit email uwu");
        legitEmails.Add("legit email2 electric boogaloo");

        phishingEmails.Add("test1");
        phishingEmails.Add("test2");
        phishingEmails.Add("test69");

        //add both lists together
        mixedEmails.AddRange(legitEmails);
        mixedEmails.AddRange(phishingEmails);

        //shuffle the combined list 
        mixedEmails = gameManager.ListShuffle(mixedEmails);

        //trim list to n items
        if (mixedEmails.Count > totalEmailsToShow)
        {
            mixedEmails.RemoveRange(totalEmailsToShow, mixedEmails.Count - totalEmailsToShow);
        }
        answerList = new List<bool>(new bool[totalEmailsToShow]);
        correctAnswerList = new List<bool>(new bool[totalEmailsToShow]);

        for (int i = 0; i < totalEmailsToShow; i++) //create the correct answer list by checking if each item in the mixed emails list is from the legit email list or the phishing email list
        {
            if (legitEmails.Contains(mixedEmails[i]))
            {
                correctAnswerList[i] = false;
            }
            else
            {
                correctAnswerList[i] = true;
            }
        }

        emailIndex = 0;
        UpdateScreen();

    }

    private void DisplayEmail()
    {
        DeleteAllText();
        emailObj = gameManager.InstantiateText(screenObject, textPrefab, currentEmail);
        gameManager.FitToSize(screenObject, emailObj);
        //Debug.Log("delete and display text");
    }

    private void DeleteAllText()
    {
        //find the old text and destroy it
        TextMeshProUGUI[] textboxList = screenObject.GetComponentsInChildren<TextMeshProUGUI>().Where(child => child.gameObject.name == "text(Clone)").ToArray();
        foreach (TextMeshProUGUI text in textboxList) //delete all text prefabs from the screen
        {
            Destroy(text.gameObject);
        }
    }

    void Update()
    {
        if (!timer.timerRunning)
        {
            DeleteAllText();
            MarkAnswers();
            EndMinigame();//may need to put end minigame into a coroutine like how i have StartCoroutine(AfterFlashing()) in the soceng puzzle
        }

        if (acceptInput)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (answerList[emailIndex]) //if the current email has been marked as true, corresponding to phishing
                {
                    answerList[emailIndex] = false; //mark as legit email
                    UpdateButton(legitColour, "Legit Email");
                }
                else //if the current email has been marked as false, corresponding to legit
                {
                    answerList[emailIndex] = true; //mark as phishing email
                    UpdateButton(phishingColour, "Phishing Email");
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                HandleArrows(-1);//navigate to the previous email in the selected emails list, if any
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                HandleArrows(1);//navigate to the next email in the selected emails list, if any 
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (answerList.SequenceEqual(correctAnswerList))
                {
                    winState = true;
                }
                else
                {
                    winState = false;
                }

                DeleteAllText();
                MarkAnswers(); //may need to put end minigame into a coroutine like how i have StartCoroutine(AfterFlashing()) in the soceng puzzle
                EndMinigame();
            }
        }
    }

    private void HandleArrows(int increment)
    {
        emailIndex = gameManager.modInt(emailIndex + increment, totalEmailsToShow);

        //reset button colour according to what the user set it to
        if (answerList[emailIndex])
        { //if set to true, corresponding to phishing
            UpdateButton(phishingColour, "Phishing Email");
        }
        else //if set to false, corresponding to legit
        {
            UpdateButton(legitColour, "Legit Email");
        }

        //Debug.Log("email index: " + emailIndex);
        UpdateScreen();
    }

    private void UpdateScreen()
    {
        currentEmail = mixedEmails[emailIndex];
        DisplayEmail();
        counterText.GetComponent<TextMeshProUGUI>().text = "Emails left:" + (totalEmailsToShow - emailIndex - 1);
    }

    private void UpdateButton(Color32 colour, string text)
    {
        answerButton.image.color = colour;
        answerButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    private void MarkAnswers() //why is it not working?
    {
        Debug.Log("marking answers");
        for (int i = 0; i < totalEmailsToShow; i++)
        {
            currentEmail = mixedEmails[i];
            DisplayEmail(); //(already called delete all text at the start of this method so no need to call it within the loop)

            if (answerList[i] == correctAnswerList[i])
            {
                StartCoroutine(gameManager.FlashText(emailObj, Color.white, Color.green)); // flash text green
            }
            else
            {
                StartCoroutine(gameManager.FlashText(emailObj, Color.white, Color.red)); //flash text red
            }
        }
        DeleteAllText();
    }
}
