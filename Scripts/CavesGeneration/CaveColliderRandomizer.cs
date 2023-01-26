using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveColliderRandomizer : MonoBehaviour
{
    // Start is called before the first frame update
    private PolygonCollider2D CaveCollider { get; set; }
    void Awake()
    {
        CaveCollider = GetComponent<PolygonCollider2D>();

        Vector2[] points = new Vector2[Random.Range(10, 16)];

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = new Vector2(Random.Range(-30f, 30f), Random.Range(-30f, 30f));
        }

        CaveCollider.points = points;
        CaveCollider.SetPath(0, points);
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
