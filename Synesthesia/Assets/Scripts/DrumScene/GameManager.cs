using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class GameManager : MonoBehaviour
{

    public int gameStage = 0;

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public XRRig rig; 
    public GameObject StageZero; 
    public GameObject StageOne;
    public bool nextStage;
    public bool stageThreeWin = false;
    public Vector3 stageTwoRigPosition; 

    public CrossFade crossFade;

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
        Stage0_1, Stage2, Stage3, Stage4
    }

    public void RetrySecondStage()
    {
        curSceneIndex -= 1;
        gameStage -= 1; 
        StartCoroutine(LoadScene());
    }

    void NextScene()
    {
        curSceneIndex += 1;
        StartCoroutine(LoadScene());
        //SceneManager.LoadScene(sceneArr.GetValue(curSceneIndex).ToString());
    }

    IEnumerator LoadScene()
    {

        crossFade.StartAnimation(); 

        yield return new WaitForSeconds(crossFade.transitionAnimationTime);

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneArr.GetValue(curSceneIndex).ToString());
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");
            yield return null;
        }
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
        else if(gameStage == 2)
        {
            stageTwoRigPosition = rig.transform.position;
            NextScene(); 
        }
        else if(gameStage == 3)
        {
            NextScene(); 
        }
        else if(gameStage == 4)
        {
            NextScene();
        }
    }

    public void ThirdStage(bool win)
    {
        if(win)
        {
            stageThreeWin = true; 
        }

        NextStage(); 
    }

    void Update()
    {
        if(nextStage)
        {
            NextStage();
            nextStage = false; 
        }
    }

    public void QuitGame()
    {
        StartCoroutine(QuitGameCoroutine()); 

    }

    IEnumerator QuitGameCoroutine()
    {
        Debug.Log("User has quit!");

        crossFade.StartAnimation();
        yield return new WaitForSeconds(crossFade.transitionAnimationTime);

        Application.Quit();
    }
}
