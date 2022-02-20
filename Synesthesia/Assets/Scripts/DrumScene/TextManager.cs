using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextManager : MonoBehaviour
{

    private static TextManager _instance;

    public static TextManager Instance { get { return _instance; } }

    public TextMeshProUGUI textDisplay;

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

    public void DisplayImage(int imageNum, float showDuration)
    {
        StartCoroutine(DisplayImageCoroutine(imageNum, showDuration));
    }

    IEnumerator DisplayImageCoroutine(int imageNum, float showDuration)
    {
        this.transform.GetChild(imageNum).gameObject.SetActive(true);

        yield return new WaitForSeconds(showDuration);

        this.transform.GetChild(imageNum).gameObject.SetActive(false);
    }
    public void ClearText()
    {
        textDisplay.text = ""; 
    }

    public void WriteText(string text)
    {
        textDisplay.text = text;
    }

    public void WriteText(string text, float duration)
    {
        StartCoroutine(WriteTextDuration(text, duration)); 
    }

    IEnumerator WriteTextDuration(string text, float duration)
    {
        WriteText(text); 

        yield return new WaitForSeconds(duration);

        ClearText(); 
    }
}
