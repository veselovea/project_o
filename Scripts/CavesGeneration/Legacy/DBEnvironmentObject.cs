using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBEnvironmentObject
{
    public List<Cave> Caves { get; set; }
    public List<POI> POIs { get; set; }
    public List<Resource> Resources { get; set; }
    public string AreaPosition { get; set; }
}

public class DBCave
{
    public string Original { get; set; }
    public Vector2[] Points { get; set; }
    public string Position { get; set; }
}

public class DBPOI
{
    public string Original { get; set; }
    public List<Tuple<string, GameObject>> Blocks { get; set; }
    public List<string> AffectedChunks { get; set; }
    public string Position { get; set; }
}

public class DBResource
{
    public string Original { get; set; }
    public string Position { get; set; }
    public Vector2[] Points { get; set; }
    public string ResourceType { get; set; }
}