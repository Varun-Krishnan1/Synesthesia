using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageOne : MonoBehaviour
{
    private static StageOne _instance;

    public static StageOne Instance { get { return _instance; } }

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

    void Start()
    {
        Debug.Log("Stage One Started!");
        AudioManager.Instance.StartTheme(); 
    }
}
