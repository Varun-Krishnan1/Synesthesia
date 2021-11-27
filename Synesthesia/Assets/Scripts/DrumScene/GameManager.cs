using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int gameStage = 0; 

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetGameStage()
    {
        return gameStage; 
    }
}
