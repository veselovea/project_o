using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkObject
{
    public Tuple<int, int> ChunkPosition { get; set; }
    public Vector3 ChunkFloorPosition { get; set; }
    public GameObject ChunkFloorOriginal { get; set; }
    public GameObject ChunkFloorClone { get; set; }
    public List<ChunkBlock> ChunkBlocks { get; set; }
    public List<EnemyOnChunk> Enemies { get; set; }
}

public class ChunkBlock
{
    public Vector3 Position { get; set; }
    public GameObject Original { get; set; }
    public GameObject Clone { get; set; }
}

public class EnemyOnChunk
{
    public GameObject Original { get; set; }
    public GameObject Clone { get; set; }
    public Vector3 Position { get; set; }
}
