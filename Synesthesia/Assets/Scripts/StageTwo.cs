using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageTwo : MonoBehaviour
{
    private static StageTwo _instance;

    public static StageTwo Instance { get { return _instance; } }

    public Cannon cannon;
    public GameObject cannonBalls; 

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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrumHit(Drum drum)
    {

        Drum.drumTypes drumType = drum.drumType; 

        if(drum.CorrectHit())
        {
            cannon.Fire(); 
        }
    }
}
