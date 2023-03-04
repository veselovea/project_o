using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegacyCaveGenerator : MonoBehaviour
{
    private List<ChunkObject> GeneratedChunks { get; set; } = new();
    private List<Tuple<int, int>> ShowedChunks { get; set; } = new();
    private Camera CameraMain { get; set; }
    private NavMeshGenerator NMG { get; set; }
    private List<Vector3> GenerationPoints { get; set; } = new List<Vector3>
    {
        new Vector3(0,0,0),     //1
        new Vector3(10,0,0),    //2
        new Vector3(10,10,0),   //3
        new Vector3(0,10,0),    //4
        new Vector3(-10,10,0),  //5
        new Vector3(-10,0,0),   //6
        new Vector3(-10,-10,0), //7
        new Vector3(0,-10,0),   //8
        new Vector3(10,-10,0),  //9
        new Vector3(20,0,0),    //10
        new Vector3(-20,0,0),   //11
        new Vector3(20,10,0),   //12
        new Vector3(-20,10,0),  //13
        new Vector3(20,-10,0),  //14
        new Vector3(-20,-10,0)  //15
    };

    public GameObject commonStoneBlock;
    public GameObject chunkFloor;
    public GameObject caveCollider;

    public List<GameObject> POIs = new();

    // Start is called before the first frame update
    void Awake()
    {
        CameraMain = Camera.main;
        NMG = GameObject.Find("NavMesh").GetComponent<NavMeshGenerator>();

        Instantiate(caveCollider, Vector3.zero, Quaternion.identity);

        lastCheckCameraPosition = Tuple.Create(0, 0);
    }

    bool isCooldown = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isCooldown == false)
        {
            StartCoroutine(CoolDown());

            CheckCameraPosition();
        }
    }

    IEnumerator CoolDown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(0.1f);
        isCooldown = false;
    }

    private Tuple<int, int> currentCheckCameraPosition;
    private Tuple<int, int> lastCheckCameraPosition;

    private void CheckCameraPosition()
    {
        bool isNewChunkGenerated = false;

        Vector3 cameraPosition = CameraMain.transform.position;

        int ceiledX = (int)Math.Ceiling(cameraPosition.x / 10);
        int ceiledY = (int)Math.Ceiling(cameraPosition.y / 10);

        if (cameraPosition.x <= 0)
        {
            ceiledX--;
        }

        if (cameraPosition.y <= 0)
        {
            ceiledY--;
        }

        currentCheckCameraPosition = Tuple.Create(ceiledX, ceiledY);

        if (currentCheckCameraPosition.Item1 != lastCheckCameraPosition.Item1 || currentCheckCameraPosition.Item2 != lastCheckCameraPosition.Item2)
        {
            Vector3 generationPoint;

            foreach (Vector3 point in GenerationPoints)
            {
                generationPoint = CameraMain.transform.TransformPoint(point);

                ceiledX = (int)Math.Ceiling(generationPoint.x / 10);
                ceiledY = (int)Math.Ceiling(generationPoint.y / 10);

                if (generationPoint.x <= 0)
                {
                    ceiledX--;
                }

                if (generationPoint.y <= 0)
                {
                    ceiledY--;
                }

                Tuple<int, int> currentChunkPosition = Tuple.Create(ceiledX, ceiledY);

                ChunkObject currentChunk = GeneratedChunks.Find(c => c.ChunkPosition.Item1 == currentChunkPosition.Item1 && c.ChunkPosition.Item2 == currentChunkPosition.Item2);

                if (currentChunk == null)
                {
                    GenerateChunk(currentChunkPosition);
                    isNewChunkGenerated = true;
                }
                else if (!ShowedChunks.Contains(currentChunkPosition))
                {
                    RegenerateChunk(currentChunk);
                }

                if (!ShowedChunks.Contains(currentChunkPosition))
                {
                    ShowedChunks.Add(currentChunkPosition);
                }
            }

            CheckShowedChunks();

            if(isNewChunkGenerated == true)
            {
                TryToGenerateCave();

                TryToGeneratePOI();

                StartCoroutine(RegenerateNavMesh());
            }
        }

        lastCheckCameraPosition = currentCheckCameraPosition;
    }

    IEnumerator RegenerateNavMesh()
    {
        yield return new WaitForSeconds(1f);
        NMG.GenerateNavMesh();
    }

    private void TryToGenerateCave()
    {
        float roll = UnityEngine.Random.Range(1, 100);
        if (roll > 70)
        {
            roll = UnityEngine.Random.Range(1, 100);
            int multiplier1;
            int multiplier2;

            if (roll > 50)
            {
                multiplier1 = 1;
            }
            else
            {
                multiplier1 = -1;
            }

            roll = UnityEngine.Random.Range(1, 100);

            if (roll > 50)
            {
                multiplier2 = 1;
            }
            else
            {
                multiplier2 = -1;
            }

            Vector3 spawnPoint = CameraMain.transform.TransformPoint(
                new Vector3(
                    UnityEngine.Random.Range(20, 40) * multiplier1,
                    UnityEngine.Random.Range(20, 40) * multiplier2,
                    0
                    )
                );

            int ceiledX = (int)Math.Ceiling(spawnPoint.x / 10);
            int ceiledY = (int)Math.Ceiling(spawnPoint.y / 10);

            if (spawnPoint.x <= 0)
            {
                ceiledX--;
            }

            if (spawnPoint.y <= 0)
            {
                ceiledY--;
            }

            if (GeneratedChunks.Find(c => c.ChunkPosition.Item1 == ceiledX && c.ChunkPosition.Item2 == ceiledY) == null)
            {
                Instantiate(caveCollider, spawnPoint, Quaternion.identity);
            }
        }
    }

    private void TryToGeneratePOI()
    {
        float roll = UnityEngine.Random.Range(1, 100);
        if (roll > 70)
        {
            roll = UnityEngine.Random.Range(1, 100);
            int multiplier1;
            if (roll > 50)
            {
                multiplier1 = 1;
            }
            else
            {
                multiplier1 = -1;
            }

            int multiplier2;

            roll = UnityEngine.Random.Range(1, 100);

            if (roll > 50)
            {
                multiplier2 = 1;
            }
            else
            {
                multiplier2 = -1;
            }

            roll = UnityEngine.Random.Range(0, POIs.Count);

            Vector3 spawnPosition = CameraMain.transform.TransformPoint
                (
                new Vector3
                    (
                        UnityEngine.Random.Range(30, 60) * multiplier1,
                        UnityEngine.Random.Range(30, 60) * multiplier2,
                        0
                    )
                );

            spawnPosition.x = Mathf.Round(spawnPosition.x);
            spawnPosition.y = Mathf.Round(spawnPosition.y);
            spawnPosition.z = 0;

            int ceiledX = (int)Math.Ceiling(spawnPosition.x / 10);
            int ceiledY = (int)Math.Ceiling(spawnPosition.y / 10);

            if (spawnPosition.x <= 0)
            {
                ceiledX--;
            }

            if (spawnPosition.y <= 0)
            {
                ceiledY--;
            }

            if(GeneratedChunks.Find(c => c.ChunkPosition.Item1 == ceiledX && c.ChunkPosition.Item2 == ceiledY) == null)
            {
                Instantiate(POIs[(int)roll], spawnPosition, Quaternion.identity);
            }
        }
    }

    private void CheckShowedChunks()
    {
        bool isInside;

        List<Tuple<int, int>> showedChunksToRemove = new();

        foreach (Tuple<int, int> showedChunk in ShowedChunks)
        {
            isInside = false;
            foreach (Vector3 point in GenerationPoints)
            {
                Vector3 worldPoint = CameraMain.transform.TransformPoint(point);

                int ceiledX = (int)Math.Ceiling(worldPoint.x / 10);
                int ceiledY = (int)Math.Ceiling(worldPoint.y / 10);

                if (worldPoint.x <= 0)
                {
                    ceiledX--;
                }

                if (worldPoint.y <= 0)
                {
                    ceiledY--;
                }

                if (showedChunk.Item1 == ceiledX && showedChunk.Item2 == ceiledY)
                {
                    isInside = true;
                    break;
                }
            }

            if (isInside == false)
            {
                if (GeneratedChunks.Find(c => c.ChunkPosition.Item1 == showedChunk.Item1 && c.ChunkPosition.Item1 == showedChunk.Item1) != null)
                {
                    ChunkObject chunk = GeneratedChunks.Find(c => c.ChunkPosition.Item1 == showedChunk.Item1 && c.ChunkPosition.Item2 == showedChunk.Item2);
                    
                    StartCoroutine(HideChunk(chunk));

                    showedChunksToRemove.Add(showedChunk);
                }
            }
        }

        foreach (Tuple<int, int> showedChunk in showedChunksToRemove)
        {
            ShowedChunks.Remove(showedChunk);
        }
    }

    IEnumerator HideChunk(ChunkObject chunk)
    {
        if(chunk.ChunkFloorClone != null)
        {
            Destroy(chunk.ChunkFloorClone);
        }

        int counter = 0;
        foreach (ChunkBlock block in chunk.ChunkBlocks)
        {
            counter++;

            if(counter % 50 == 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

            Destroy(block.Clone);
        }

        yield return new WaitForSeconds(2f);

        if (!ShowedChunks.Contains(chunk.ChunkPosition))
        {
            foreach (ChunkBlock block in chunk.ChunkBlocks)
            {
                counter++;

                if (counter % 50 == 0)
                {
                    yield return new WaitForSeconds(0.1f);
                }

                Destroy(block.Clone);
            }
        }
    }

    private void GenerateChunk(Tuple<int, int>chunkPosition)
    {
        float startPointX = chunkPosition.Item1 * 10;
        float startPointY = chunkPosition.Item2 * 10;

        float chunkCenterX;
        float chunkCenterY;

        if (startPointX < 0)
        {
            chunkCenterX = startPointX + 5;
            startPointX += 0.5f;
        }
        else
        {
            chunkCenterX = startPointX - 5;
            startPointX -= 0.5f;
        }

        if (startPointY < 0)
        {
            chunkCenterY = startPointY + 5;
            startPointY += 0.5f;
        }
        else
        {
            chunkCenterY = startPointY - 5;
            startPointY -= 0.5f;
        }

        ChunkObject newChunk = new();

        GeneratedChunks.Add(newChunk);

        newChunk.ChunkPosition = chunkPosition;

        newChunk.ChunkFloorPosition = new Vector3(chunkCenterX, chunkCenterY, 0);
        newChunk.ChunkFloorOriginal = chunkFloor;
        newChunk.ChunkFloorClone = Instantiate(chunkFloor, new Vector3(chunkCenterX, chunkCenterY, 0), Quaternion.identity);

        newChunk.ChunkBlocks = new();

        ChunkBlock chunkBlock;

        float originalStartPointX = startPointX;

        for (int x = 0; x < 10; x++)
        {
            startPointX = originalStartPointX;
            for (int y = 0; y < 10; y++)
            {
                Collider2D hitCollider = Physics2D.OverlapCircle(new Vector2(startPointX, startPointY), 0f);
                if (hitCollider == null)
                {
                    chunkBlock = new();

                    chunkBlock.Position = new Vector3(startPointX, startPointY, 0);
                    
                    chunkBlock.Original = commonStoneBlock;

                    newChunk.ChunkBlocks.Add(chunkBlock);
                }
                else
                {
                    POIBuilder poi = hitCollider.GetComponent<POIBuilder>();
                    if (poi != null)
                    {
                        foreach (Tuple<Vector3, GameObject> POIblock in poi.BuildInChunk(chunkPosition))
                        {
                            chunkBlock = new();

                            chunkBlock.Position = POIblock.Item1;

                            chunkBlock.Original = POIblock.Item2;

                            newChunk.ChunkBlocks.Add(chunkBlock);
                        }
                    }
                }

                if (startPointX < 0)
                {
                    startPointX++;
                }
                else
                {
                    startPointX--;
                }
            }

            if (startPointY < 0)
            {
                startPointY++;
            }
            else
            {
                startPointY--;
            }
        }

        StartCoroutine(InstantiateBlocksInChunk(newChunk));
    }

    private void RegenerateChunk(ChunkObject chunk)
    {
        chunk.ChunkFloorClone = Instantiate(chunk.ChunkFloorOriginal, chunk.ChunkFloorPosition, Quaternion.identity);
        StartCoroutine(InstantiateBlocksInChunk(chunk));
    }

    IEnumerator InstantiateBlocksInChunk(ChunkObject chunk)
    {
        int counter = 0;

        foreach (ChunkBlock block in chunk.ChunkBlocks)
        {
            Collider2D hitCollider = Physics2D.OverlapCircle(new Vector2(block.Position.x, block.Position.y), 0f);

            if(hitCollider != null)
            {
            POIBuilder poi = hitCollider.GetComponent<POIBuilder>();
            if (poi != null)
            {
                poi.BuildInChunk(chunk.ChunkPosition);
            }
            }

            counter++;

            if(counter % 50 == 0)
            {
                yield return new WaitForSeconds(0.1f);
            }

            block.Clone = Instantiate(block.Original, block.Position, Quaternion.identity);
        }
    }
}
