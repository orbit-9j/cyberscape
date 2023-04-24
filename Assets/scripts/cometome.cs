using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//my friend wrote this for fun, not part of the project, remind me to delete it
public class cometome : MonoBehaviour
{
    [SerializeField] private GameObject puzzlenpcs;
    [SerializeField] private GameObject playerchar;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private bool increasespeed = false;
    private List<GameObject> robots;
    private List<Transform> robotsTrans;
    // Start is called before the first frame update
    void Start()
    {
        robots = new List<GameObject>();
        robotsTrans = new List<Transform>();
        // foreach (var gameObj in FindObjectsOfType(typeof(GameObject)) as GameObject[])
        // {
        //     // if (gameObj.name.Contains("round robot"))
        //     if (gameObj.name.Contains("clue"))
        //     {
        //         robots.Add(gameObj);
        //     }
        // }

        foreach (Transform child in puzzlenpcs.transform)
        {
            robots.Add(child.gameObject);
            robotsTrans.Add(child);
        }

        foreach (GameObject robot in robots)
        {
            Debug.Log(robot.name);
        }

    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime * 0.1f;


        foreach (Transform trans in robotsTrans)
        {
            trans.position = Vector2.MoveTowards(trans.position, playerchar.transform.position, step * Random.Range(0, 10));
        }

        if (increasespeed)
            speed = speed + 0.01f;
    }
}
