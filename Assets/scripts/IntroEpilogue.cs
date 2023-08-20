using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEpilogue : intro
{
    [SerializeField] private TextAsset EpilogueJSON;

    protected override void Start() //not updating, needs some sort of reset method
    {
        bool epilogue = PlayerPrefs.GetInt("endGame", 0) == 1;
        if (epilogue)
        {
            JumpToFile(EpilogueJSON);
        }
        else
        {
            JumpToFile(inkJSON);
        }
    }


}
