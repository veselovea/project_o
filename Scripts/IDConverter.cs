using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class IDConverter : MonoBehaviour
{
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    public List<GameObject> Gameobjects = new();

    public GameObject IDToGameobjectConverter(string id)
    {
        return Gameobjects.Find(go => go.name == id);
    }

    public string GameobjectToIDConverter(GameObject gameObject)
    {
        try
        {
            return Gameobjects.Find(go => go == gameObject).name;
        }
        catch
        {
            return null;
        }
    }

    public DBChunkObject ConvertChunkToIds(ChunkObject Chunk)
    {
        DBChunkObject dbChunk = new();

        dbChunk.ChunkFloorPosition = Chunk.ChunkPosition.Item1 + "," + Chunk.ChunkPosition.Item2;
        dbChunk.ChunkFloorPosition = Chunk.ChunkFloorPosition.x + "," + Chunk.ChunkFloorPosition.y + "," + Chunk.ChunkFloorPosition.z;
        dbChunk.ChunkFloorOriginal = GameobjectToIDConverter(Chunk.ChunkFloorOriginal);
        dbChunk.ChunkBlocks = new();
        
        foreach (ChunkBlock block in Chunk.ChunkBlocks)
        {
            DBChunkBlock dbBlock = new();

            dbBlock.Original = GameobjectToIDConverter(block.Original);
            dbBlock.Position = block.Position.x + "," + block.Position.y + "," + block.Position.z;

            dbChunk.ChunkBlocks.Add(dbBlock);
        }

        dbChunk.Enemies = new();

        foreach (EnemyOnChunk enemy in Chunk.Enemies)
        {
            DBEnemyOnChunk dbEnemy = new();

            dbEnemy.Original = GameobjectToIDConverter(enemy.Original);
            dbEnemy.Position = enemy.Position.x + "," + enemy.Position.y + "," + enemy.Position.z;

            dbChunk.Enemies.Add(dbEnemy);
        }

        return dbChunk;
    }

    public ChunkObject ConvertIdsToChunk(DBChunkObject dbChunk)
    {
        ChunkObject chunk = new();

        chunk.ChunkPosition = Tuple.Create(Convert.ToInt32(dbChunk.ChunkPosition.Split(',')[0]), Convert.ToInt32(dbChunk.ChunkPosition.Split(',')[1]));
        chunk.ChunkFloorPosition = new Vector3(
            (float)Convert.ToDouble(dbChunk.ChunkFloorPosition.Split(',')[0]),
            (float)Convert.ToDouble(dbChunk.ChunkFloorPosition.Split(',')[1]),
            (float)Convert.ToDouble(dbChunk.ChunkFloorPosition.Split(',')[2]));
        chunk.ChunkFloorOriginal = IDToGameobjectConverter(dbChunk.ChunkFloorOriginal);
        chunk.ChunkBlocks = new();

        foreach (DBChunkBlock dbBlock in dbChunk.ChunkBlocks)
        {
            ChunkBlock block = new();

            block.Original = IDToGameobjectConverter(dbBlock.Original);
            block.Position = new Vector3(
                (float)Convert.ToDouble(dbBlock.Position.Split(',')[0]),
                (float)Convert.ToDouble(dbBlock.Position.Split(',')[1]),
                (float)Convert.ToDouble(dbBlock.Position.Split(',')[2]));

            chunk.ChunkBlocks.Add(block);
        }

        chunk.Enemies = new();

        foreach (DBEnemyOnChunk dbEnemy in dbChunk.Enemies)
        {
            EnemyOnChunk enemy = new();

            enemy.Original = IDToGameobjectConverter(dbEnemy.Original);
            enemy.Position = new Vector3(
                (float)Convert.ToDouble(dbEnemy.Position.Split(',')[0]),
                (float)Convert.ToDouble(dbEnemy.Position.Split(',')[1]),
                (float)Convert.ToDouble(dbEnemy.Position.Split(',')[2]));

            chunk.Enemies.Add(enemy);
        }

        return chunk;
    }
}
