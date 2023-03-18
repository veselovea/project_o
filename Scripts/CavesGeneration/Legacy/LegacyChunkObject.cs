using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyChunkObject
{
    public Tuple<int, int> ChunkPosition { get; set; }
    public Vector3 ChunkFloorPosition { get; set; }
    public GameObject ChunkFloorOriginal { get; set; }
    public GameObject ChunkFloorClone { get; set; }
    public List<ChunkBlock> ChunkBlocks { get; set; }

    public List<EnemyOnChunk> Enemies { get; set; }
}

public class LegacyChunkBlock
{
    public Vector3 Position { get; set; }
    public GameObject Original { get; set; }
    public GameObject Clone { get; set; }
}

public class LegacyEnemyOnChunk
{
    public GameObject Original { get; set; }
    public GameObject Clone { get; set; }
    public Vector3 Position { get; set; }
}
