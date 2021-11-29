using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{
    public string[] sentences;
    public float[] timingBetweenSentences;
    public int[] numSentencesPerActivation; 
    public TextMeshProUGUI textDisplay;
    public Camera mainCamera; 

    private int curIndex;
    private int curActivationIndex; 
    private float curTime;
    private bool activated; 
    // Start is called before the first frame update
    void Start()
    {
        activated = true; 
        curIndex = 0; 
        textDisplay.text = sentences[curIndex];
        curTime = timingBetweenSentences[curIndex]; 
    }

    // Update is called once per frame
    void Update()
    {
        curTime -= Time.deltaTime; 
        if(curTime <= 0 && activated)
        {
            // reached end of sentences 
            if(curIndex+1 == numSentencesPerActivation[curActivationIndex])
            {
                textDisplay.text = ""; 
                activated = false;
                curActivationIndex += 1; 
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

    public void Activate()
    {
        activated = true;
        curIndex += 1; 
    }
}
