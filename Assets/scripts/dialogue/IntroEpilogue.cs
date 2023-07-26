using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEpilogue : SceneStart
{
    [SerializeField] private TextAsset EpilogueJSON;

    protected override void Start()
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
