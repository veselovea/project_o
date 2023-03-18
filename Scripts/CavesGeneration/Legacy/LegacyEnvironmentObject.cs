using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyEnvironmentObject
{
    public List<Cave> Caves { get; set; }
    public List<POI> POIs { get; set; }
    public List<Resource> Resources { get; set; }
    public Tuple<int, int> AreaPosition { get; set; }
}

public class LegacyCave
{
    public GameObject Original { get; set; }
    public GameObject Clone { get; set; }
    public Vector2[] Points { get; set; }
    public Vector3 Position { get; set; }
}

public class LegacyPOI
{
    public GameObject Original { get; set; }
    public GameObject Clone { get; set; }
    public List<Tuple<Vector3, GameObject>> Blocks { get; set; }
    public List<Tuple<int, int>> AffectedChunks { get; set; }
    public Vector3 Position { get; set; }
}

public class LegacyResource
{
    public GameObject Original { get; set; }
    public GameObject Clone { get; set; }
    public Vector3 Position { get; set; }
    public Vector2[] Points { get; set; }
    public GameObject ResourceType { get; set; }
}