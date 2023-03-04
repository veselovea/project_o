using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentObject
{
    public List<Cave> Caves { get; set; }
    public List<POI> POIs { get; set; }
    public Tuple<int, int> AreaPosition { get; set; }
}

public class Cave
{
    public GameObject Original { get; set; }
    public GameObject Clone { get; set; }
    public Vector2[] Points { get; set; }
    public Vector3 Position { get; set; }
}

public class POI
{
    public GameObject Original { get; set; }
    public GameObject Clone { get; set; }
    public List<Tuple<Vector3, GameObject>> Blocks { get; set; }
    public List<Tuple<int, int>> AffectedChunks { get; set; }
    public Vector3 Position { get; set; }
}
