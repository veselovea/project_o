using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySampleLogic : MonoBehaviour
{
    NavMeshAgent Agent { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ActivateAgent());
        //Agent.SetDestination(new Vector3(0, 0, 0));
    }

    IEnumerator ActivateAgent()
    {
        yield return new WaitForSeconds(1f);
        Agent = GetComponent<NavMeshAgent>();
        Agent.enabled = true;
        Agent.updateUpAxis = false;
        Agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
