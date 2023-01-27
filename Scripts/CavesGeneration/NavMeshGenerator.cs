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
}
