using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemMovement : MonoBehaviour
{
    public GameObject targetObject;
    public float moveSpeed = 1f;

    private bool speedEstablished; 
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if(targetObject)
        {
            Vector3 targetPosition = targetObject.transform.position;
            float maxDistance = Vector3.Distance(transform.position, targetPosition);

            if (!speedEstablished)
            {
                moveSpeed = maxDistance / 4;
                speedEstablished = true;
            }

            if (targetObject.tag == "Sky")
            {
                targetPosition = targetObject.transform.position + new Vector3(0f, 100f, 0f);
            }


            this.transform.LookAt(targetPosition);
            Vector3 directionToMove = targetPosition - transform.position;
            directionToMove = directionToMove.normalized * Time.deltaTime * moveSpeed;
            transform.position = transform.position + Vector3.ClampMagnitude(directionToMove, maxDistance);

            // particle reaches object 
            if (maxDistance < .05)
            {
                targetObject.SetActive(true);
                Debug.Log(targetObject.GetComponentsInChildren<Transform>());
                foreach (Transform child in targetObject.GetComponentsInChildren<Transform>(true)) // use true parameter to get inactive
                {
                    Debug.Log(child);
                    child.gameObject.SetActive(true);
                }

                Destroy(this.gameObject);
            }
        }
        
    }
}
