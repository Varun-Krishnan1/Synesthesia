using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LowPolyUnderwaterPack;

public class StageTwo : MonoBehaviour
{
    private static StageTwo _instance;

    public static StageTwo Instance { get { return _instance; } }

    public UserShip userShip;
    public EnemyShip enemyShip; 
    public WaterMesh waterMesh;

    public float startDelay;
    public float maxWave1Scale;
    public float wave1ScaleProgression; 
    public float maxWave1Speed;
    public float wave1SpeedProgression;

    private float curWave1Scale;
    private float curWave1Speed;

    private int totalComboLength;
    private int curComboLength;
    private bool inCombo;

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
        StartCoroutine(StartScene()); 
    }

    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(startDelay); 
        curWave1Scale = waterMesh.waveAmplitude1;
        curWave1Speed = waterMesh.waveSpeed1;

        BeatManager.Instance.Activate();
        AudioManager.Instance.StartStageTheme(2);

        enemyShip.activated = true; 
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ComboStarted(int comboLength)
    {
        inCombo = true;
        totalComboLength = comboLength; 
    }

    // -- only called by last combo note 
    public void ComboEnded()
    {
        if(curComboLength >= totalComboLength)
        {
            TextManager.Instance.WriteText("Combo Succeeded!", 3f);
            userShip.Shoot();
        }
        else
        {
            TextManager.Instance.WriteText("Combo Failed!", 3f); 
            Debug.Log("Needed: " + totalComboLength);
            Debug.Log("Achieved: " + curComboLength); 
            // -- cannon not firing sound 
        }
        inCombo = false;
        curComboLength = 0; 
    }

    public void DrumHit(Drum drum)
    {

        Drum.drumTypes drumType = drum.drumType;
        Note noteHit = drum.CorrectHit();

        if (drumType == Drum.drumTypes.Kick)
        {
            userShip.SetShield();
        }
        else if(noteHit)
        {
            Debug.Log("Correct Hit!");
            if(inCombo && noteHit.isComboNote)
            {
                curComboLength += 1;
                Debug.Log("Combo Incremented: " + curComboLength);
            }
            else
            {
                userShip.Shoot();
            }
        }
    }

    public void IncreaseStageIntensity()
    {
        curWave1Scale += wave1ScaleProgression;
        curWave1Scale = Mathf.Clamp(curWave1Scale, curWave1Scale, maxWave1Scale);

        curWave1Speed += wave1SpeedProgression;
        curWave1Speed = Mathf.Clamp(curWave1Speed, curWave1Speed, maxWave1Speed); 
        
        waterMesh.waveSpeed1 = curWave1Speed;
        waterMesh.waveSpeed2 = curWave1Speed * 2f;

        waterMesh.waveAmplitude1 = curWave1Scale;
        waterMesh.waveAmplitude2 = curWave1Scale / 5f;
    }

    public void StageOver(bool isUserShip)
    {
        if(isUserShip)
        {
            GameManager.Instance.NextStage(); 
        }
    }
}
