using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject player;

    void Awake()
    {
        Instance = this;
    }

    //https://www.youtube.com/watch?v=ii31ObaAaJo 12/02/2023
    //public SaveManager saveManager;

    /* void Start()
    {
        saveManager.Load();
    } */

    //button save/load
    /* public void ClickSave()
    {
        saveManager.Save();
    } */

    //key save/load
    //https://www.youtube.com/watch?v=6uMFEM-napE 12/02/2023
    /* private void Update()
    {
        saveManager.data.position_x = player.transform.position.x;
        saveManager.data.position_y = player.transform.position.y;

        if (Input.GetKeyDown(KeyCode.G))
        {
            saveManager.Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            saveManager.Load();
        }
    } */

}
