using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGenerator : MonoBehaviour
{
    NavMeshSurface NavigationSurface { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        NavigationSurface = GetComponent<NavMeshSurface>();
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public void GenerateNavMesh()
    {
        NavigationSurface.BuildNavMesh();
    }

    public void DelayedGenerateNavMesh(float seconds)
    {
        StartCoroutine(DGNM(seconds));
    }

    private IEnumerator DGNM(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        NavigationSurface.BuildNavMesh();
    }
}
