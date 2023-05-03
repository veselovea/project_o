using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CaveGenerator : MonoBehaviour
{
    private List<ChunkObject> GeneratedChunks { get; set; } = new();
    private List<Tuple<int, int>> ShowedChunks { get; set; } = new();

    private List<EnvironmentObject> GeneratedEnvironmentAreas { get; set; } = new();
    private List<Tuple<int, int>> ActiveEnvironmentAreas { get; set; } = new();

    private List<EnemyOnChunk> ActiveEnemies { get; set; } = new();

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

    private List<Vector3> EnvironmentPoints { get; set; } = new List<Vector3>
    {
        new Vector3(50,50,0),     //1
        new Vector3(-50,50,0),    //2
        new Vector3(50,-50,0),    //3
        new Vector3(-50,-50,0),   //4
    };

    public GameObject commonStoneBlock;
    public GameObject chunkFloor;
    public GameObject caveCollider;
    public GameObject resourceCollider;

    public List<GameObject> POIs = new();
    public List<GameObject> Resources = new();
    public List<GameObject> Enemies = new();

    // Start is called before the first frame update
    void Awake()
    {
        CameraMain = Camera.main;
        NMG = GameObject.Find("NavMesh").GetComponent<NavMeshGenerator>();

        //GameObject firstCave = Instantiate(caveCollider, Vector3.zero, Quaternion.identity);
        //firstCave.GetComponent<CaveColliderRandomizer>().GenerateColliderPoints();

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

    public void DeleteBlock(GameObject block)
    {
        Vector3 blockPosirion = block.transform.position;

        int ceiledX = (int)Math.Ceiling(blockPosirion.x / 10);
        int ceiledY = (int)Math.Ceiling(blockPosirion.y / 10);

        if (blockPosirion.x <= 0)
        {
            ceiledX--;
        }

        if (blockPosirion.y <= 0)
        {
            ceiledY--;
        }

        ChunkObject currentBlockChunck = GeneratedChunks.Find(c => c.ChunkPosition.Item1 == ceiledX && c.ChunkPosition.Item2 == ceiledY);

        currentBlockChunck.ChunkBlocks.Find(b => b.Position == block.transform.position).Original = null;
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
            Vector3 environmentPoint;

            foreach (Vector3 point in EnvironmentPoints)
            {
                environmentPoint = CameraMain.transform.TransformPoint(point);

                ceiledX = (int)Math.Ceiling(environmentPoint.x / 100);
                ceiledY = (int)Math.Ceiling(environmentPoint.y / 100);

                if (environmentPoint.x <= 0)
                {
                    ceiledX--;
                }

                if (environmentPoint.y <= 0)
                {
                    ceiledY--;
                }

                Tuple<int, int> currentEnvironmentAreaPosition = Tuple.Create(ceiledX, ceiledY);

                EnvironmentObject currentArea = GeneratedEnvironmentAreas.Find(e => e.AreaPosition.Item1 == ceiledX && e.AreaPosition.Item2 == ceiledY);

                if (currentArea == null)
                {
                    GenerateEnvironmentArea(currentEnvironmentAreaPosition);
                }
                else if (!ActiveEnvironmentAreas.Contains(currentEnvironmentAreaPosition))
                {
                    RegenerateEnvironmentArea(currentArea);
                }

                if (!ActiveEnvironmentAreas.Contains(currentEnvironmentAreaPosition))
                {
                    ActiveEnvironmentAreas.Add(currentEnvironmentAreaPosition);
                }
            }

            CheckActiveAreas();

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

                ChunkObject currentChunk = GeneratedChunks.Find(c => c.ChunkPosition.Item1 == ceiledX && c.ChunkPosition.Item2 == ceiledY);

                if (currentChunk == null)
                {
                    GenerateChunk(currentChunkPosition);
                    isNewChunkGenerated = true;
                }
                else if (!ShowedChunks.Contains(currentChunkPosition))
                {
                    RegenerateChunk(currentChunk);
                    isNewChunkGenerated = true;
                }

                if (!ShowedChunks.Contains(currentChunkPosition))
                {
                    ShowedChunks.Add(currentChunkPosition);
                }
            }

            CheckShowedChunks();

            if(isNewChunkGenerated == true)
            {
                StartCoroutine(RegenerateNavMesh());

                CheckActiveEnemies();
            }
        }

        lastCheckCameraPosition = currentCheckCameraPosition;
    }

    IEnumerator RegenerateNavMesh()
    {
        yield return new WaitForSeconds(1.5f);

        NMG.GenerateNavMesh();
    }

    private void CheckActiveEnemies()
    {
        List<EnemyOnChunk> EnemiesToDelete = new();

        foreach (EnemyOnChunk enemy in ActiveEnemies)
        {
            if (enemy.Clone == null)
            {
                enemy.Original = null;
                EnemiesToDelete.Add(enemy);
            }
            else if (Vector2.Distance(enemy.Clone.transform.position, CameraMain.transform.position) > 40)
            {
                Destroy(enemy.Clone);
                EnemiesToDelete.Add(enemy);
            }
        }

        foreach (EnemyOnChunk enemy in EnemiesToDelete)
        {
            ActiveEnemies.Remove(enemy);
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
        if (chunk.ChunkFloorClone != null)
        {
            Destroy(chunk.ChunkFloorClone);
        }

        int counter = 0;
        foreach (ChunkBlock block in chunk.ChunkBlocks)
        {
            counter++;

            //if(counter % 50 == 0)
            //{
            //    yield return new WaitForSeconds(0.1f);
            //}

            yield return new WaitForSeconds(0.001f);

            if (block.Clone != null)
            {
                Destroy(block.Clone);
            }
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
                if (block.Clone != null)
                {
                    Destroy(block.Clone);
                }
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
        newChunk.Enemies = new();

        ChunkBlock chunkBlock;
        EnemyOnChunk enemy;

        float originalStartPointX = startPointX;

        for (int x = 0; x < 10; x++)
        {
            startPointX = originalStartPointX;
            for (int y = 0; y < 10; y++)
            {
                Collider2D hitCaveCollider = Physics2D.OverlapCircle(new Vector2(startPointX, startPointY), 0f, LayerMask.GetMask("Caves"));
                Collider2D hitResourceCollider = Physics2D.OverlapCircle(new Vector2(startPointX, startPointY), 0f, LayerMask.GetMask("Resources"));
                Collider2D hitPOICollider = Physics2D.OverlapCircle(new Vector2(startPointX, startPointY), 0f, LayerMask.GetMask("POIs"));

                if(hitCaveCollider == null && hitResourceCollider == null && hitPOICollider == null)
                {
                    chunkBlock = new();

                    chunkBlock.Position = new Vector3(startPointX, startPointY, 0);

                    chunkBlock.Original = commonStoneBlock;

                    newChunk.ChunkBlocks.Add(chunkBlock);
                }

                if (hitPOICollider != null)
                {
                    POIBuilder poi = hitPOICollider.GetComponent<POIBuilder>();

                    foreach (Tuple<Vector3, GameObject> POIblock in poi.BuildInChunk(chunkPosition))
                    {
                        chunkBlock = new();

                        chunkBlock.Position = POIblock.Item1;

                        chunkBlock.Original = POIblock.Item2;

                        newChunk.ChunkBlocks.Add(chunkBlock);
                    }
                }

                if(hitPOICollider == null && hitResourceCollider != null)
                {
                    ResourceColliderRandomizer rcr = hitResourceCollider.GetComponent<ResourceColliderRandomizer>();

                    chunkBlock = new();

                    chunkBlock.Position = new Vector3(startPointX, startPointY, 0);

                    chunkBlock.Original = rcr.ResourceType;

                    newChunk.ChunkBlocks.Add(chunkBlock);
                }

                if(hitCaveCollider != null && hitResourceCollider == null)
                {
                    int roll = UnityEngine.Random.Range(0, 100);

                    if (roll >= 99)
                    {
                        int enemyNumber = UnityEngine.Random.Range(0, Enemies.Count);

                        enemy = new();
                        enemy.Original = Enemies[enemyNumber];
                        enemy.Position = new Vector3(startPointX, startPointY, 0);

                        newChunk.Enemies.Add(enemy);
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
            counter++;

            //if(counter % 50 == 0)
            //{
            //    yield return new WaitForSeconds(0.1f);
            //}

            yield return new WaitForSeconds(0.001f);

            if (block.Original != null)
            {
                block.Clone = Instantiate(block.Original, block.Position, Quaternion.identity);
            }
        }

        foreach (EnemyOnChunk enemy in chunk.Enemies)
        {
            if (enemy.Clone == null && enemy.Original != null)
            {
                enemy.Clone = Instantiate(enemy.Original, enemy.Position, Quaternion.identity);
                ActiveEnemies.Add(enemy);
            }
        }
    }

    private void GenerateEnvironmentArea(Tuple<int, int> areaPosition)
    {
        float startX = areaPosition.Item1 * 100; 
        float startY = areaPosition.Item2 * 100;

        float endX;
        float endY;

        if(startX < 0)
        {
            startX += 10;
            endX = startX + 80;
        }
        else
        {
            startX -= 10;
            endX = startX - 80;
        }

        if(startY < 0)
        {
            startY += 10;
            endY = startY + 80;
        }
        else
        {
            startY -= 10;
            endY = startY - 80;
        }

        EnvironmentObject newArea = new();
        newArea.AreaPosition = areaPosition;
        newArea.Caves = new();
        newArea.POIs = new();
        newArea.Resources = new();

        for (int i = 0; i < 5; i++)
        {
            int roll = UnityEngine.Random.Range(0, 100);

            if (roll >= 50)
            {
                Vector3 position = new Vector3(UnityEngine.Random.Range((int)startX, (int)endX), UnityEngine.Random.Range((int)startY, (int)endY), 0);

                Cave newCave = new();

                newCave.Original = caveCollider;
                newCave.Position = position;
                newCave.Clone = Instantiate(newCave.Original, newCave.Position, Quaternion.identity);
                CaveColliderRandomizer cavColldier = newCave.Clone.GetComponent<CaveColliderRandomizer>();
                cavColldier.GenerateColliderPoints();
                newCave.Points = cavColldier.Points;

                newArea.Caves.Add(newCave);
            }
        }

        for (int i = 0; i < 2; i++)
        {
            int roll = UnityEngine.Random.Range(0, 100);

            if(roll >= 70)
            {
                Vector3 position = new Vector3(UnityEngine.Random.Range((int)startX, (int)endX), UnityEngine.Random.Range((int)startY, (int)endY), 0);

                int POInumber = UnityEngine.Random.Range(0, POIs.Count);

                POI newPOI = new();

                newPOI.Original = POIs[POInumber];
                newPOI.Position = position;
                newPOI.Clone = Instantiate(newPOI.Original, newPOI.Position, Quaternion.identity);
                POIBuilder poiBuilder = newPOI.Clone.GetComponent<POIBuilder>();
                poiBuilder.DetermineBlocks();
                newPOI.Blocks = poiBuilder.Blocks;
                newPOI.AffectedChunks = poiBuilder.AffectedChunks;

                newArea.POIs.Add(newPOI);
            }
        }

        for (int i = 0; i < 3; i++)
        {
            int roll = UnityEngine.Random.Range(0, 100);

            if(roll >= 50)
            {
                Vector3 position = new Vector3(UnityEngine.Random.Range((int)startX, (int)endX), UnityEngine.Random.Range((int)startY, (int)endY), 0);

                int resNumber = UnityEngine.Random.Range(0, Resources.Count);

                Resource newResource = new();

                newResource.Original = resourceCollider;
                newResource.Position = position;
                newResource.ResourceType = Resources[resNumber];
                newResource.Clone = Instantiate(newResource.Original, newResource.Position, Quaternion.identity);
                ResourceColliderRandomizer resCollider = newResource.Clone.GetComponent<ResourceColliderRandomizer>();
                resCollider.ResourceType = newResource.ResourceType;
                resCollider.GenerateColliderPoints();
                newResource.Points = resCollider.Points;

                newArea.Resources.Add(newResource);
            }
        }

        GeneratedEnvironmentAreas.Add(newArea);
    }

    private void CheckActiveAreas()
    {
        bool isInside;

        List<Tuple<int, int>> ActiveAreasToRemove = new();

        foreach (Tuple<int, int> activeArea in ActiveEnvironmentAreas)
        {
            isInside = false;
            foreach (Vector3 point in EnvironmentPoints)
            {
                Vector3 worldPoint = CameraMain.transform.TransformPoint(point);

                int ceiledX = (int)Math.Ceiling(worldPoint.x / 100);
                int ceiledY = (int)Math.Ceiling(worldPoint.y / 100);

                if (worldPoint.x <= 0)
                {
                    ceiledX--;
                }

                if (worldPoint.y <= 0)
                {
                    ceiledY--;
                }

                if (activeArea.Item1 == ceiledX && activeArea.Item2 == ceiledY)
                {
                    isInside = true;
                    break;
                }
            }

            if (isInside == false)
            {
                if (GeneratedEnvironmentAreas.Find(a => a.AreaPosition.Item1 == activeArea.Item1 && a.AreaPosition.Item1 == activeArea.Item1) != null)
                {
                    EnvironmentObject environmentArea = GeneratedEnvironmentAreas.Find(a => a.AreaPosition.Item1 == activeArea.Item1 && a.AreaPosition.Item2 == activeArea.Item2);

                    HideEnvironmentArea(environmentArea);

                    ActiveAreasToRemove.Add(activeArea);
                }
            }
        }

        foreach (Tuple<int, int> activeArea in ActiveAreasToRemove)
        {
            ActiveEnvironmentAreas.Remove(activeArea);
        }
    }

    private void HideEnvironmentArea(EnvironmentObject environmentArea)
    {
        foreach (Cave cave in environmentArea.Caves)
        {
            Destroy(cave.Clone);
        }

        foreach (POI poi in environmentArea.POIs)
        {
            if (poi.Clone != null)
            {
                poi.Blocks = poi.Clone.GetComponent<POIBuilder>().Blocks;
                poi.AffectedChunks = poi.Clone.GetComponent<POIBuilder>().AffectedChunks;
            }

            Destroy(poi.Clone);
        }

        foreach (Resource res in environmentArea.Resources)
        {
            Destroy(res.Clone);
        }
    }

    private void RegenerateEnvironmentArea(EnvironmentObject environmentArea)
    {
        foreach (Cave cave in environmentArea.Caves)
        {
            GameObject regeneratedCave = Instantiate(cave.Original, cave.Position, Quaternion.identity);
            PolygonCollider2D caveCollider = regeneratedCave.GetComponent<PolygonCollider2D>();
            caveCollider.points = cave.Points;
            caveCollider.SetPath(0, cave.Points);

            cave.Clone = regeneratedCave;
        }

        foreach (POI poi in environmentArea.POIs)
        {
            GameObject regeneratedPOI = Instantiate(poi.Original, poi.Position, Quaternion.identity);
            POIBuilder poiBuilder = regeneratedPOI.GetComponent<POIBuilder>();
            poiBuilder.DestroyBlueprints();
            poiBuilder.Blocks = poi.Blocks;
            poiBuilder.AffectedChunks = poi.AffectedChunks;

            poi.Clone = regeneratedPOI;
        }

        foreach (Resource res in environmentArea.Resources)
        {
            GameObject regeneratedResource = Instantiate(res.Original, res.Position, Quaternion.identity);
            ResourceColliderRandomizer rcr = regeneratedResource.GetComponent<ResourceColliderRandomizer>();
            rcr.ResourceType = res.ResourceType;
            PolygonCollider2D resourceCollider = regeneratedResource.GetComponent<PolygonCollider2D>();
            resourceCollider.points = res.Points;
            resourceCollider.SetPath(0, res.Points);

            res.Clone = regeneratedResource;
        }
    }
}
