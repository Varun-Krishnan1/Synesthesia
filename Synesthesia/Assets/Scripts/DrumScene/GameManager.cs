using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : MonoBehaviour
{

    public int gameStage = 0;

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public GameObject StageZero; 
    public GameObject StageOne;

    private Array sceneArr;
    private int curSceneIndex = 0; 

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        sceneArr = Enum.GetValues(typeof(Scene));

        Scene curScene = (Scene)Enum.Parse(typeof(Scene), SceneManager.GetActiveScene().name);
        curSceneIndex = Array.IndexOf(sceneArr, curScene);
    }
    
    private enum Scene
    {
        Ship, Stage2, Stage3Lose
    }

    void NextScene()
    {
        curSceneIndex += 1;
        SceneManager.LoadScene(sceneArr.GetValue(curSceneIndex).ToString());
    }

    public int GetGameStage()
    {
        return gameStage; 
    }

    public void NextStage()
    {
        gameStage += 1; 
        Debug.Log("Next Stage!");
        if(gameStage == 1)
        {
            StageOne.SetActive(true);
        }
        else if(gameStage == 2 || gameStage == 3)
        {
            NextScene(); 
        }
    }
}
