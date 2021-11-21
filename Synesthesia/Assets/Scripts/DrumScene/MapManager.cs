using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    private static MapManager _instance;

    public static MapManager Instance { get { return _instance; } }

    private List<GameObject> mountainArr;
    private List<bool> mountainActiveArr; 

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
        //mountainActiveArr = new List<bool>(); 
        //this.mountainArr = new List<GameObject>(GameObject.FindGameObjectsWithTag("Mountain"));
        //foreach (GameObject m in mountainArr)
        //{
        //    m.SetActive(false);
        //    mountainActiveArr.Add(false); 
        //}
    }

    public GameObject GetRandomDormantMountain()
    {
        // -- get only false values in mountainActiveArray
        List<int> indices = new List<int>();
        for (int i = 0; i < mountainActiveArr.Count; i++)
        {
            if (!mountainActiveArr[i])
            {
                indices.Add(i);
            }
        }

        // -- if no dormant mountains just return first one 
        if(indices.Count == 0)
        {
            return mountainArr[0]; 
        }

        int randomIndicesIndex = Random.Range(0, indices.Count);
        int randomDormantMountainIndex = indices[randomIndicesIndex];

        Debug.Log(randomDormantMountainIndex);
        // -- set that dormant mountain to active 
        mountainActiveArr[randomDormantMountainIndex] = true; 

        return mountainArr[randomDormantMountainIndex]; 
    }

}
