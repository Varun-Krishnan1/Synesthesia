using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public bool isComboNote;
    public Color comboNoteColor; 
    public bool isLastComboNote;
    public float destroyTime;

    // Start is called before the first frame update
    void Start()
    {
        if(isComboNote)
        {
            this.GetComponent<Renderer>().material.SetColor("Base_Color", comboNoteColor);
        }

        StartCoroutine(DestroySelf());
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
