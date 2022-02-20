using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using DG.Tweening;
using System.Collections;

public class Drum : MonoBehaviour
{
    public drumTypes drumType;
    public bool playOnButtonPress = false;
    public ActionBasedController left_controller; 
    public ActionBasedController right_controller;
    public float saturationChangeOnCorrectHit;
    public GameObject note;
    public bool isExitDrum;

    [Header("Notes")]
    public Vector3 noteSpawnOffset; 
    public float leewayTime;
    public bool hit;
    public bool spawnNote;

    private bool hasNote;
    private Note noteComponent; 
    private Collider noteCollider;
    private List<Note> spawnedNotes = new List<Note>();  

    private float previousRightTriggerValue; 
    private float previousLeftTriggerValue;
    private float lastKickHit; 

    private float scaleHeight = .01f;
    private float scaleTime = .10f;

    private AudioSource source;
    private bool drumIsActive;
    private bool scaleInProgress = false; 

    private float originalH, originalS, originalV;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        drumIsActive = true;

        Color.RGBToHSV(transform.parent.GetComponentInChildren<Renderer>().material.GetColor("Base_Color"), out originalH, out originalS, out originalV);
    }

    private void OnTriggerEnter(Collider other)
    {

        DrumStick drumstick = other.GetComponent<DrumStick>(); 
        if (drumstick != null)
        {
            DrumHit(drumstick);
        }

        Note tempNoteComponent = other.GetComponent<Note>(); 
        if(tempNoteComponent && (spawnedNotes.Contains(tempNoteComponent)))
        {
            noteComponent = tempNoteComponent; 
            hasNote = true;
            noteCollider = other;
            spawnedNotes.RemoveAt(0); 
        }
    }

    void DrumHit(DrumStick drumstick)
    {
        //Debug.Log("this y: " + transform.position.y);
        //Debug.Log("drumstick y: " + drumstick.transform.position.y);

        if (drumType == drumTypes.Kick)
        {
            // MUSIC 
            ActivateSound(drumstick.GetSpeed());

            // VISUAL 
            ScaleDrum();
            RequestVisualChange();


            // VIBRATION 
            // normalize between 0 and 1 assuming max un-normalized value is 25 
            float vibrationIntensity = drumstick.GetSpeed() / 25;
            drumstick.SendHapticImpulse(vibrationIntensity, .25f);
        }

        // make sure drumstick hitting from above only for all drums besides kick
        else if ((drumstick.gameObject.transform.position.y > this.transform.position.y))
        {
            // MUSIC 
            ActivateSound(drumstick.GetSpeed());

            // VISUAL 
            ScaleDrum();

            // -- No visual change for exit drum 
            if (isExitDrum)
            {
                StartCoroutine(ExitScene()); 
            }

            RequestVisualChange();

            if (hasNote)
            {
                StartCoroutine(CorrectHitEffect(.15f));
                drumstick.SendHapticImpulse(.75f, .25f);
            }
            else
            {
                // VIBRATION 
                // normalize between 0 and 1 assuming max un-normalized value is 25 
                float vibrationIntensity = drumstick.GetSpeed() / 25;
                drumstick.SendHapticImpulse(vibrationIntensity, .25f);
            }


        }
    }

    public IEnumerator ExitScene()
    {
        foreach (drumTypes drumType in (drumTypes[]) System.Enum.GetValues(typeof(drumTypes)))
        {
            VisualManager.Instance.DrawColorSplash(StageZero.Instance.drumTypeToLocation(drumType), drumType);
        }

        yield return new WaitForSeconds(.5f);

        GameManager.Instance.QuitGame(); 
    }

    void ScaleDrum()
    {
        if(!scaleInProgress)
        {
            scaleInProgress = true; 
            transform.parent.DOScale(transform.parent.localScale + new Vector3(scaleHeight, scaleHeight, scaleHeight), scaleTime).SetLoops(2, LoopType.Yoyo).OnComplete(() => { scaleInProgress = false; });
        }

    }

    public IEnumerator CorrectHitEffect(float animationTime)
    {
        VisualManager.Instance.DrawColorSplash(transform.position, transform.rotation, new Vector3(.1f, .1f, .1f), drumType);
        //Destroy(note.gameObject); 

        float H, S, V;
        Color.RGBToHSV(transform.parent.GetComponentInChildren<Renderer>().material.GetColor("Base_Color"), out H, out S, out V);
        
        SetDrumColor(H, S - saturationChangeOnCorrectHit, V); 

        yield return new WaitForSeconds(animationTime);

        SetDrumColor(originalH, originalS, originalV); 
    }
    
    
    void SetDrumColor(float H, float S, float V)
    {
        foreach (Renderer r in transform.parent.GetComponentsInChildren<Renderer>())
        {
            if (r != this.GetComponent<Renderer>())
            {
                foreach (Material m in r.materials)
                {
                    m.SetColor("Base_Color", Color.HSVToRGB(H, S, V));
                }
            }
        }

    }

    public void SetAlphaClip(float alphaClip)
    {
        foreach (Renderer r in transform.parent.GetComponentsInChildren<Renderer>())
        {
            if (r != this.GetComponent<Renderer>())
            {
                foreach (Material m in r.materials)
                {
                    m.SetFloat("AlphaClip", alphaClip);
                }
            }
        }
    }

    void RequestVisualChange()
    {
        if (drumIsActive)
        {
            VisualManager.Instance.RequestElementChange(this);
        }
    }


    void Update()
    {
        lastKickHit -= Time.deltaTime; 

        if(playOnButtonPress)
        {
            CheckButtonPress(); 
        }

        if(hasNote && !noteCollider)
        {
            // -- note has been destroyed 
            hasNote = false;
            noteComponent = null; 
        }

        // -- FOR TESTING 
        if (hit)
        {
            ScaleDrum();
            RequestVisualChange();
            if (hasNote)
            {
                StartCoroutine(CorrectHitEffect(.15f));
            }
            hit = false; 
        }

        if(spawnNote)
        {
            SpawnNote(false, false, -1, -1);
            spawnNote = false; 
        }
    }

    public enum drumTypes
    {
        Snare, 
        HiHat, 
        FloorTom, 
        Kick, 
        HighTom,
        MidTom

    }

    private void ActivateSound(float volume)
    {
        source.pitch = Random.Range(0.95f, 1.05f);
        source.volume = volume; 
        source.PlayOneShot(source.clip, source.volume);

    }
    void CheckButtonPress()
    {
        if(lastKickHit <= 0f)
        {
            float right_trigger_value = right_controller.activateAction.action.ReadValue<float>();
            float left_trigger_value = left_controller.activateAction.action.ReadValue<float>();

            bool buttonPressed = false;
            if (previousRightTriggerValue < 0.5f && right_trigger_value > 0.5f)
            {
                ActivateSound(right_trigger_value);
                buttonPressed = true;
            }
            else if (previousLeftTriggerValue < 0.5f && left_trigger_value > 0.5f)
            {
                ActivateSound(left_trigger_value);
                buttonPressed = true;
            }
            if (buttonPressed)
            {
                ScaleDrum();
                RequestVisualChange();

                // keep vibration intensity low 
                left_controller.SendHapticImpulse(.1f, .25f);
                right_controller.SendHapticImpulse(.1f, .25f);

                lastKickHit = .1f;
            }

            previousRightTriggerValue = right_trigger_value;
            previousLeftTriggerValue = left_trigger_value;
        }
        
    }

    public void SpawnNote(bool isComboNote, bool isLastComboNote, int curNumberInCombo, int doubleNoteSymbol)
    {
        Quaternion rotation = note.transform.rotation; 

        if(GameManager.Instance.gameStage == 2)
        {
            rotation = Quaternion.Euler(0f, 50f, 0f); 
        }

        GameObject tempNote = Instantiate(note, transform.position + noteSpawnOffset, rotation);
        Note tempNoteComponent = tempNote.GetComponent<Note>();

        Color noteColor = Color.HSVToRGB(originalH, originalS, originalV);
        noteColor.a = .69f;
        tempNoteComponent.noteColor = noteColor; 

        tempNoteComponent.isComboNote = isComboNote;
        tempNoteComponent.isLastComboNote = isLastComboNote;

        if(isComboNote)
        {
            tempNoteComponent.curNumberInCombo = curNumberInCombo;
        }
        if (doubleNoteSymbol != -1)
        {
            tempNoteComponent.doubleNoteSymbol = doubleNoteSymbol; 
        }

        //tempNote.transform.parent = this.transform.parent; 
        tempNote.transform.DOMove(this.transform.position, 2f).SetEase(Ease.Linear);

        tempNoteComponent.destroyTime = 2f + leewayTime;

        spawnedNotes.Add(tempNoteComponent); 
    }

    public Note CorrectHit()
    {
        return noteComponent;
    }
}
