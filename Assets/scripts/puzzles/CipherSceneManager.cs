using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CipherSceneManager : MonoBehaviour
{
    private GameManager gameManager;
    public int encryptionKey;
    public string plaintext = "Hey door, are you free tomorrow after six pm? I was thinking we could hang out, if you know what I mean.";
    public string ciphertext;

    [SerializeField] GameObject puzzle;


    void Start()
    {
        gameManager = GameObject.Find("game manager").GetComponent<GameManager>();
        encryptionKey = Random.Range(1, 26);
        ciphertext = gameManager.Encipher(plaintext, encryptionKey);

        puzzle.GetComponent<DialogueTrigger>().variableNames = new List<string>() { "ciphertext", "key" };
        puzzle.GetComponent<DialogueTrigger>().variableValues = new List<string>() { ciphertext, encryptionKey.ToString() };
    }


}
