using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Note : MonoBehaviour
{
    public Color noteColor; 
    public bool isComboNote;
    public bool isLastComboNote;
    public Color[] doubleNoteColors; 
    public float destroyTime;

    public int curNumberInCombo;
    public int doubleNoteSymbol; 

    public GameObject comboCircle;
    public Transform numberSlot;
    public GameObject[] numbers; 
    public int curColor = 0;
    private float comboColorChangeTime = 0.6f;
    private Material myMaterial; 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroySelf());

        GetComponent<Renderer>().material.SetColor("_BaseColor", noteColor);

        if (isComboNote)
        {
            GameObject number = Instantiate(numbers[curNumberInCombo], numberSlot.transform.position, numberSlot.transform.rotation, this.transform);
            number.transform.localScale = new Vector3(5, 5, 5);

            myMaterial = GetComponent<Renderer>().material;
            
            Material numberMaterial = number.GetComponent<Renderer>().material;
            numberMaterial.SetColor("BaseColor", Color.white); // -- dissolve material so different reference for color 

            if (doubleNoteSymbol > 0)
            {
                // numberMaterial.SetColor("_baseColor", doubleNoteColors[doubleNoteSymbol]);

                // -- CIRCLES -- // 
                // comboCircle.GetComponent<Renderer>().material.SetColor("_BaseColor", doubleNoteColors[doubleNoteSymbol]);
                // comboCircle.SetActive(true);

            }
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
