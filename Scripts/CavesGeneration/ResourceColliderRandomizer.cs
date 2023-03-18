using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceColliderRandomizer : MonoBehaviour
{
    private PolygonCollider2D ResourceCollider { get; set; }
    public Vector2[] Points { get; set; }
    public GameObject ResourceType { get; set; }
    public void GenerateColliderPoints()
    {
        ResourceCollider = GetComponent<PolygonCollider2D>();

        Points = new Vector2[6];

        for (int i = 0; i < Points.Length; i++)
        {
            Points[i] = new Vector2(Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        }

        ResourceCollider.points = Points;
        ResourceCollider.SetPath(0, Points);
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    ResourceCollider = GetComponent<PolygonCollider2D>();

    //    Points = new Vector2[6];

    //    for (int i = 0; i < Points.Length; i++)
    //    {
    //        Points[i] = new Vector2(Random.Range(-30f, 30f), Random.Range(-30f, 30f));
    //    }

    //    ResourceCollider.points = Points;
    //    ResourceCollider.SetPath(0, Points);
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
