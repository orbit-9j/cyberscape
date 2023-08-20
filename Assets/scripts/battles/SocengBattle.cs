using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SocengBattle : BattleMinigame
{
    [SerializeField] protected GameObject textPrefab; //i think i only need it for soceng battle
    private string name;
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

    private Color32 legitColour = new Color32(180, 120, 0, 255); //yellow-orange
    private Color32 phishingColour = new Color32(113, 89, 133, 255); //ashy purple

    //text to display score (how many emails you got right)
    //ideally i'd need to show which ones were wrong and right
    //make a method for the end of the minigame that iterates through the mixed emails arrays, and compares the values for each index between answer list and correct answer list and then flash the text red/green 
    //move the text flash method to game manager and use that

    void Start()
    {
        base.Start();
        name = gameManager.playerName;
        legitEmails.AddRange(new string[] {
            "from: sharkhubbot@sharkhub.com \nto:" + name + "@geezmail.com \nDear " + name + ", \nWe have noticed that your SharkHub account has been accessed from a new location. If this wasn't you, please ensure you are logged in and click the link below to manage your devices: [link]. \nKeep Swimming! \nSharkHub security bot",
            "from: orders@rainforest.com \nto:"+name+"@geezmail.com \nHello " + name + "! \nThank you for your recent purchase of Blue Stuffed Shark Plushie Soft Toy For Kids Perfect Gift For Girl Boy Wife Girlfriend Birthday. Your order has been received and is being processed for shipment. You will receive a further email when your order has been shipped. This email is automated; please do not reply to this email. \nRainforest Order Bot",
            "from: accounts@payfriend.com \nto:" + name + "@geezmail.com \nHeads up! \nHere is your monthly account activity summary for the month of May. Please review your transactions and let us know if you notice any unauthorized activity. If you have any questions, please contact our customer support team. \nPayFriend: Paying Made Easy",
            "from: subscriptions@gamevault.com \nto:" + name + "@geezmail.com \nWhat's up GamerGrill400 \nWe would like to remind you that your subscription to GameVault is set to renew on June 1st, 2035. If you would like to make any changes to your subscription, please log into your account and make the necessary adjustments.\nGameVault subscriptions bot \nGameVault: pay as you game!",
            "from: jessejames@yeehaw.com \nto:" + name + "@geezmail.com \nHi " + name + "! \nI saw you try to pull a push door on campus today and I thought that was really endearing so I was wondering if you wanted to grab a coffee at the crossroads sometime tomorrow? ;) \nJesse (from robopsych class!)",
            "from: research@STU.edu \nto:" + name + "@geezmail.com \nHi all! \nLaura, an undergraduate at the School of Social Sciences, is inviting you to participate in a research study looking into the effects of playing Cavebuild on students' ability to juggle oranges. Please contact her at laurak@STU.edu if you wish to participate in this study. \nSubterropolis University Research Committee",
            //3 more
            "from: newsletter@flicksfix.com \nto:" + name + "@geezmail.com \nHappy Monday, " + name + "! \nHere are this week's picks for your favourite genres: Tales From Robot Therapy, 2050: A Space Oddity, Fight or Flight Club, Jung at Heart, Psychic K. Tune in next week for more great recommendations! \nFlicksFix Newsletter",
            "from: reminders@zenzone.com  \nto:" + name + "@geezmail.com \nStop! Pause! Think! \nOur systems show that you have missed your recommended Head Empty time today. This is your reminder to turn off your microchip for at least 5 minutes to download the latest updates and adverts! Disconnecting from the world wide web has been proven to be beneficial to maintaining the health of your Flesh Prison so it will serve you longer. \nZenZone",
            "from: notifications@st.gov \nto:" + name + "@geezmail.com \nDear Resident, \nThe council of Subterropolis has updated the number of thoughts allowed per citizen per day from 469 to 420. For more information on what this may mean for you, please visit [link]. \nStay sheeple \nSubterropolis Council"
    });

        phishingEmails.AddRange(new string[] {
            "from: gamevault@geezmail.com \nto:"+name+"@geezmail.com \nHello User, \nOur records indicate that your account password is outdated and needs to be updated immediately. Click here [link] to enter a new password now. Failure to do so may result in account suspension. \nThank you. \nGameVault Suppot Teem",
            "from:security@bokface.com \nto:"+name+"@geezmail.com \nHello, we have received reports of unusual activity on your account. To secure your account, please click on the link below to change your password immediately. \n[link] \nBookFace Security Team",
            "from: orders@rainforest.com \nto:"+name+"@geezmail.com \nDear customer, \nYour recent order is ready for shipping. Please click on the link below to confirm your shipping address and track your package. \n[link] \nRainfroest Order Bot",
            "from: luckylottery@ll.com \nto:"+name+"@geezmail.com \nCongratulations! You have been entered into a prize draw and you have won £100,000! Clink on the link to claim your prize now: [link]",
            "from: martinsmith629@yeehaw.com \nto:"+name+"@geezmail.com \nHello old friend! \nI know I haven't seen you since high school but I wanted to let you know that someone has uploaded a very embarrassing video of you online! I can get a hacker I know to remove it if you pay me £100 as a friend tip :) \nMartin",
            "from: security@payfriends.com \nto:" + name + "@geezmail.com, \nWatch out! \nOur system has detected suspicious activity on your account. To prevent your account from being closed, please click on the link below and enter your account information to prove your identity. \n[link] \n\nPayFriend: Paying Made Easy",
            "from: hiring@sharkhub.com \nto:" + name + "@geezmail.com, \nWe are pleased to offer you a job opportunity at SharkHub. Please click on the link below to view the job offer and provide your CV and personal information for a background check. \n[link] \nKeep Swimming! \nSharkHub Hiring Team",
            "from: ishaansingh@yeehaw.com \nto:" + name + "@geezmail.com, \nHi, my name is John from FlicksFix and I am writing you to inform you of an error in your subscription payment. Your payment cannot be processed and your account will be shut down if you choose to ignore this message. To resolve the error, please buy a £15 FlicksFix gift card and reply to this email with the card's code to pay for your subscription this way. \nThank You \nJohn from FlicksFix",
            "from: orders@rainforest.com \nto:" + name + "@geezmail.com, \nSorry, we missed you! \nYour Rainforest package scheduled for delivery today has not been delivered because you were not home. Please click on the link below and enter your name and phone number to re-schedule delivery to a different time. \n[link] \nRainforest Order Bot"
    });
        ResetMinigame();

    }
    public override void ResetMinigame()
    {
        answerButton.enabled = true;

        totalEmailsToShow = 3;

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
            //MarkAnswers();
            winState = false;
            EndMinigame();//may need to put end minigame into a coroutine like how i have StartCoroutine(AfterFlashing()) in the soceng puzzle
        }

        if (acceptInput)
        {
            if (Input.GetKeyDown(KeyCode.E))
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
                //MarkAnswers(); //may need to put end minigame into a coroutine like how i have StartCoroutine(AfterFlashing()) in the soceng puzzle
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
        //Debug.Log("marking answers");
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
