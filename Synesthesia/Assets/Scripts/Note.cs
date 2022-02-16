using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Note : MonoBehaviour
{
    public bool isComboNote;
    public Color comboNoteColor; 
    public bool isLastComboNote;
    public Color[] doubleNoteColors; 
    public float destroyTime;

    public int curColor = 0;
    private float comboColorChangeTime = 0.6f;
    private Material myMaterial; 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySelf());
        if(isComboNote)
        {
            myMaterial = GetComponent<Renderer>().material; 
            InvokeRepeating("ChangeColor", 0, comboColorChangeTime); 
        }
    }

    void ChangeColor()
    {
        Debug.Log("Here");
        myMaterial.DOColor(doubleNoteColors[curColor], "_BaseColor", comboColorChangeTime);
        curColor += 1; 
        if(curColor == doubleNoteColors.Length)
        {
            curColor = 0; 
        }
    }


    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(destroyTime);

        if(isLastComboNote)
        {
            StageTwo.Instance.ComboEnded(); 
        }
        Destroy(this.gameObject);
    }

}
