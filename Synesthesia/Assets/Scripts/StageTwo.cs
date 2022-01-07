using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LowPolyUnderwaterPack;

public class StageTwo : MonoBehaviour
{
    private static StageTwo _instance;

    public static StageTwo Instance { get { return _instance; } }

    public UserShip userShip;
    public WaterMesh waterMesh;

    public float maxWave1Scale;
    public float wave1ScaleProgression; 
    public float maxWave1Speed;
    public float wave1SpeedProgression;

    private float curWave1Scale;
    private float curWave1Speed;   
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
        curWave1Scale = waterMesh.waveAmplitude1;
        curWave1Speed = waterMesh.waveSpeed1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrumHit(Drum drum)
    {

        Drum.drumTypes drumType = drum.drumType; 

        if(drumType == Drum.drumTypes.Kick)
        {
            userShip.SetShield();
        }
        else if(drum.CorrectHit())
        {
            userShip.Shoot(); 
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
