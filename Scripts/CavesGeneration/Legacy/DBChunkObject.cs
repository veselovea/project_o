using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBChunkObject
{
    public string ChunkPosition { get; set; }
    public string ChunkFloorPosition { get; set; }
    public string ChunkFloorOriginal { get; set; }
    public List<DBChunkBlock> ChunkBlocks { get; set; }
    public List<DBEnemyOnChunk> Enemies { get; set; }
}

public class DBChunkBlock
{
    public string Position { get; set; }
    public string Original { get; set; }
}

public class DBEnemyOnChunk
{
    public string Original { get; set; }
    public string Position { get; set; }
}
