using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class CaveGenerator : MonoBehaviour
{
    private List<Tuple<int, int>> GeneratedChunkNumbers { get; set; } = new();
    private List<Tuple<int, int>> ShowedChunks { get; set; } = new();
    private List<Tuple<Tuple<int, int>, List<Tuple<Vector3, GameObject>>>> GeneratedChunks { get; set; } = new();
    private Camera CameraMain { get; set; }
    public GameObject commonStoneBlock;
    public GameObject chunkFloor;
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

    //private List<Tuple<int, int>> LastCheckPoints { get; set; } = new List<Tuple<int, int>>
    //{
    //    new Tuple<int, int>(0,0),   //1
    //    new Tuple<int, int>(0,0),   //2
    //    new Tuple<int, int>(0,0),   //3
    //    new Tuple<int, int>(0,0),   //4
    //    new Tuple<int, int>(0,0),   //5
    //    new Tuple<int, int>(0,0),   //6
    //    new Tuple<int, int>(0,0),   //7
    //    new Tuple<int, int>(0,0),   //8
    //    new Tuple<int, int>(0,0),   //9
    //    new Tuple<int, int>(0,0),   //10
    //    new Tuple<int, int>(0,0),   //11
    //    new Tuple<int, int>(0,0),   //12
    //    new Tuple<int, int>(0,0),   //13
    //    new Tuple<int, int>(0,0),   //14
    //    new Tuple<int, int>(0,0)    //15
    //};

    //private List<Tuple<int, int>> CurrentCheckPoints { get; set; } = new List<Tuple<int, int>>
    //{
    //    new Tuple<int, int>(0,0),   //1
    //    new Tuple<int, int>(0,0),   //2
    //    new Tuple<int, int>(0,0),   //3
    //    new Tuple<int, int>(0,0),   //4
    //    new Tuple<int, int>(0,0),   //5
    //    new Tuple<int, int>(0,0),   //6
    //    new Tuple<int, int>(0,0),   //7
    //    new Tuple<int, int>(0,0),   //8
    //    new Tuple<int, int>(0,0),   //9
    //    new Tuple<int, int>(0,0),   //10
    //    new Tuple<int, int>(0,0),   //11
    //    new Tuple<int, int>(0,0),   //12
    //    new Tuple<int, int>(0,0),   //13
    //    new Tuple<int, int>(0,0),   //14
    //    new Tuple<int, int>(0,0)    //15
    //};
    // Start is called before the first frame update
    void Awake()
    {
        CameraMain = Camera.main;
        NMG = GameObject.Find("NavMesh").GetComponent<NavMeshGenerator>();
    }

    bool isCoolDown = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if(isCoolDown == false)
        {
            StartCoroutine(CoolDown());

            StartCoroutine(CheckCameraPosition());

            CheckCameraMovement();

            CheckShowedChunks();
        }
    }

    IEnumerator CoolDown()
    {
        isCoolDown = true;
        yield return new WaitForSeconds(0.2f);
        isCoolDown = false;
    }

    int startCounter = 0;

    IEnumerator CheckCameraPosition()
    {
        int ceiledX;
        int ceiledY;

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

            Tuple<int, int> cameraChunk = Tuple.Create(ceiledX, ceiledY);

            if (!GeneratedChunkNumbers.Contains(cameraChunk))
            {
                ShowedChunks.Add(cameraChunk);

                GeneratedChunkNumbers.Add(cameraChunk);

                yield return new WaitForSeconds(0.2f);

                StartCoroutine(GenerateChunk(cameraChunk));
            }
            else if (!ShowedChunks.Contains(cameraChunk) && GeneratedChunks.Find(c => c.Item1.Item1 == cameraChunk.Item1 && c.Item1.Item2 == cameraChunk.Item2) != null)
            {
                ShowedChunks.Add(cameraChunk);

                StartCoroutine(ReGenerateChunk(cameraChunk));

                //Debug.Log("Add " + cameraChunk.Item1 + "|||" + cameraChunk.Item2);
                //Debug.Log("---------------------------------------------------------------");
            }
        }

    }


    private Tuple<int, int> lastCameraChunk = null;
    private void CheckShowedChunks()
    {
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

        Tuple<int, int> currentCameraChunk = Tuple.Create(ceiledX, ceiledY);

        if (currentCameraChunk != lastCameraChunk)
        {
            bool isInside;

            List<Tuple<int, int>> showedChunksToRemove = new();
            foreach (Tuple<int, int> showedChunk in ShowedChunks)
            {
                isInside = false;
                foreach (Vector3 point in GenerationPoints)
                {
                    Vector3 worldPoint = CameraMain.transform.TransformPoint(point);

                    ceiledX = (int)Math.Ceiling(worldPoint.x / 10);
                    ceiledY = (int)Math.Ceiling(worldPoint.y / 10);

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

                if(isInside == false)
                {
                    //Debug.Log("--------------------------------------------------------------- " + showedChunk.Item1 + "|||" + showedChunk.Item2);
                    if (GeneratedChunks.Find(c => c.Item1.Item1 == showedChunk.Item1 && c.Item1.Item2 == showedChunk.Item2) != null)
                    {
                        //Debug.Log("---------------------------------------------------------------");
                        List<Tuple<Vector3, GameObject>> currentChunk = GeneratedChunks.Find(c => c.Item1.Item1 == showedChunk.Item1 && c.Item1.Item2 == showedChunk.Item2).Item2;

                        foreach (Tuple<Vector3, GameObject> block in currentChunk)
                        {
                            Destroy(block.Item2);
                        }
                    }

                    showedChunksToRemove.Add(showedChunk);
                }
            }

            foreach (Tuple<int, int> showedChunk in showedChunksToRemove)
            {
                ShowedChunks.Remove(showedChunk);
            }
        }

        lastCameraChunk = currentCameraChunk;
    }

    private void CheckCameraMovement()
    {
        //int counter = 0;
        //Vector3 generationPoint;
        //int ceiledX;
        //int ceiledY;

        //foreach (Vector3 point in GenerationPoints)
        //{
        //    generationPoint = CameraMain.transform.TransformPoint(point);
        //    ceiledX = (int)Math.Ceiling(generationPoint.x / 10);
        //    ceiledY = (int)Math.Ceiling(generationPoint.y / 10);

        //    if (generationPoint.x <= 0)
        //    {
        //        ceiledX--;
        //    }

        //    if (generationPoint.y <= 0)
        //    {
        //        ceiledY--;
        //    }

        //    Tuple<int, int> cameraChunk = Tuple.Create(ceiledX, ceiledY);
        //    //CurrentCheckPoints[counter] = cameraChunk;

        //    if (LastCheckPoints[counter].Item1 != cameraChunk.Item1 || LastCheckPoints[counter].Item2 != cameraChunk.Item2)
        //    {
        //        //Debug.Log("---------------------------------------------------------------------" + LastCheckPoints[counter] + "|||" + cameraChunk);

        //        Debug.Log("---------------------------------------------------------------------");

        //        LastCheckPoints[counter] = cameraChunk;

        //        if (GeneratedChunks.Find(c => c.Item1.Item1 == cameraChunk.Item1 && c.Item1.Item2 == cameraChunk.Item2) != null)
        //        {
        //            List<Tuple<Vector3, GameObject>> currentChunk = GeneratedChunks.Find(c => c.Item1.Item1 == cameraChunk.Item1 && c.Item1.Item2 == cameraChunk.Item2).Item2;
        //            foreach (Tuple<Vector3, GameObject> block in currentChunk)
        //            {
        //                Destroy(block.Item2);
        //            }
        //        }
        //    }

        //    //if (CurrentCheckPoints[counter] != LastCheckPoints[counter])
        //    //{
        //    //    //cameraChunk.Item1 != LastCheckPoints[counter].Item1 && cameraChunk.Item2 != LastCheckPoints[counter].Item2
        //    //    //GeneratedChunks.Find(c => c.Item1.Item1 == cameraChunk.Item1 && c.Item1.Item2 == cameraChunk.Item2) != null
        //    //    Debug.Log("---------------------------------------------------------------------");
        //    //    if (startCounter >= 15)
        //    //    {
        //    //        List<Tuple<Vector3, GameObject>> currentChunk = GeneratedChunks.Find(c => c.Item1.Item1 == cameraChunk.Item1 && c.Item1.Item2 == cameraChunk.Item2).Item2;
        //    //        foreach (Tuple<Vector3, GameObject> block in currentChunk)
        //    //        {
        //    //            Destroy(block.Item2);
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        startCounter++;
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    //Debug.Log("---------------------------------------------------------------------" + CurrentCheckPoints[counter] + "|||" + LastCheckPoints[counter]);
        //    //}

        //    counter++;
        //}

        ////LastCheckPoints = CurrentCheckPoints;
    }

    IEnumerator GenerateChunk(Tuple<int, int> newChunkNumber)
    {
        int ceiledX = newChunkNumber.Item1;
        int ceiledY = newChunkNumber.Item2;

        float totalX = ceiledX * 10;
        float totalY = ceiledY * 10;

        float centerX;
        float centerY;

        if (totalX < 0)
        {
            centerX = totalX + 5;
            totalX += 0.5f;
        }
        else
        {
            centerX = totalX - 5;
            totalX -= 0.5f;
        }

        float originalTotalX = totalX;

        if (totalY < 0)
        {
            centerY = totalY + 5;
            totalY += 0.5f;
        }
        else
        {
            centerY = totalY - 5;
            totalY -= 0.5f;
        }

        List<Tuple<Vector3, GameObject>> newChunk = new();

        GameObject newChunkFloor = Instantiate(chunkFloor, new Vector3(centerX, centerY, 0), Quaternion.identity);

        yield return new WaitForSeconds(0.01f);

        newChunk.Add(new Tuple<Vector3, GameObject>(new Vector3(centerX, centerY, 0), newChunkFloor));

        for (int i = 0; i < 10; i++)
        {
            totalX = originalTotalX;
            for (int j = 0; j < 10; j++)
            {
                Collider2D hitCollider = Physics2D.OverlapCircle(new Vector2(totalX, totalY), 0f);
                if (hitCollider == null)
                {
                    Vector3 blockPosition = new Vector3(totalX, totalY, 0);
                    GameObject newBlock = Instantiate(commonStoneBlock, blockPosition, Quaternion.identity);

                    newChunk.Add(new Tuple<Vector3, GameObject>(blockPosition, newBlock));
                }

                if (totalX < 0)
                {
                    totalX++;
                }
                else
                {
                    totalX--;
                }
            }

            if (totalY < 0)
            {
                totalY++;
            }
            else
            {
                totalY--;
            }

            yield return new WaitForSeconds(0.01f);
        }

        NMG.GenerateNavMesh();

        GeneratedChunks.Add(new Tuple<Tuple<int, int>, List<Tuple<Vector3, GameObject>>>(newChunkNumber, newChunk));
    }

    IEnumerator ReGenerateChunk(Tuple<int, int> ChunkNumber)
    {
        //Debug.Log("Regenerate " + ChunkNumber.Item1 + "|||" + ChunkNumber.Item2);
        if (GeneratedChunks.Find(c => c.Item1.Item1 == ChunkNumber.Item1 && c.Item1.Item2 == ChunkNumber.Item2) != null)
        {
            //Debug.Log("---------------------------------------------------------------");
            List<Tuple<Vector3, GameObject>> currentChunk = GeneratedChunks.Find(c => c.Item1.Item1 == ChunkNumber.Item1 && c.Item1.Item2 == ChunkNumber.Item2).Item2;
            GeneratedChunks.Remove(GeneratedChunks.Find(c => c.Item1.Item1 == ChunkNumber.Item1 && c.Item1.Item2 == ChunkNumber.Item2));

            int counter = 0;

            GameObject newBlock;

            List<Tuple<Vector3, GameObject>> regeneratedChunk = new();

            foreach (Tuple<Vector3, GameObject> block in currentChunk)
            {
                if(counter % 50 == 0)
                {
                    yield return new WaitForSeconds(0.2f);
                }

                if (counter == 0)
                {
                    newBlock = Instantiate(chunkFloor, block.Item1, Quaternion.identity);
                    regeneratedChunk.Add(new Tuple<Vector3, GameObject>(block.Item1, newBlock));
                }
                else
                {
                    newBlock = Instantiate(commonStoneBlock, block.Item1, Quaternion.identity);

                    regeneratedChunk.Add(new Tuple<Vector3, GameObject>(block.Item1, newBlock));
                }

                counter++;
            }

            GeneratedChunks.Add(new Tuple<Tuple<int, int>, List<Tuple<Vector3, GameObject>>>(ChunkNumber, regeneratedChunk));
        }

        yield return new WaitForSeconds(0.1f);

        NMG.GenerateNavMesh();
    }
}
