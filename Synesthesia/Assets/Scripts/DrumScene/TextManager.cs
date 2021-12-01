using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{

    private static TextManager _instance;

    public static TextManager Instance { get { return _instance; } }

    public string[] sentences;
    public float[] timingBetweenSentences;
    public int[] sentenceElementToStopActivation; 
    public TextMeshProUGUI textDisplay;
    public Camera mainCamera; 

    private int curIndex;
    private int curActivationIndex; 
    private float curTime;
    private bool activated;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        activated = true; 
        curIndex = 0; 
        textDisplay.text = sentences[curIndex];
        curTime = timingBetweenSentences[curIndex]; 

        // Variable Checks 
        if(timingBetweenSentences.Length != sentences.Length - 1)
        {
            Debug.LogError("Mismatch between length of timingBetweenSentences[] and sentences[]");
        }
    }

    // Update is called once per frame
    void Update()
    {
        curTime -= Time.deltaTime; 
        if(curTime <= 0 && activated)
        {
            // reached end of sentences 
            if(curIndex+1 == sentenceElementToStopActivation[curActivationIndex])
            {
                DeActivate(); 
            }
            else
            {
                curIndex += 1;
                textDisplay.text = sentences[curIndex];
                curTime = timingBetweenSentences[curIndex-1];
            }
        }

        //RaycastHit hit;

        //// if raycast hits, it checks if it hit an object with the tag Player
        //Debug.DrawRay(mainCamera.gameObject.transform.position, transform.forward, Color.green);

        //if (Physics.Raycast(mainCamera.gameObject.transform.position, transform.forward, out hit))
        //{
        //    Debug.Log(hit.collider.gameObject);
        //    TextManager t = hit.collider.gameObject.GetComponent<TextManager>();
        //    if (t)
        //    {
        //        Debug.Log("Looking at!");
        //    }
        //}
    }

    void DeActivate()
    {
        Debug.Log("End of Activation!");
        textDisplay.text = "";
        activated = false;
        curActivationIndex += 1;

        if(GameManager.Instance.GetGameStage() == 0)
        {
            StageZero.Instance.NextLevel(); 
        }
    }

    public void Activate()
    {
        activated = true;
    }
}
