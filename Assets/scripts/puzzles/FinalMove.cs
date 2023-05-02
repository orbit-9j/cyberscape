using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalMove : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject elevatorButton;
    [SerializeField] private Button button;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite pressedSprite;
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private string knotName;

    void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(PressButton());
        }
    }

    private IEnumerator PressButton()
    {
        gameManager.PressUnpressButton(button, defaultSprite, pressedSprite);
        yield return new WaitForSeconds(1.0f);
        PlayerPrefs.SetInt("endGame", true ? 1 : 0);
        PlayerPrefs.Save();
        elevatorButton.GetComponent<BoxCollider2D>().enabled = true;
        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        DialogueManager.GetInstance().JumpToKnot(knotName, new List<string>() { "playerName", "consoleName" }, new List<string>() { gameManager.playerName, gameManager.consoleName });
        gameObject.SetActive(false);
    }
}
