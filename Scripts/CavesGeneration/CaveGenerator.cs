using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CaveGenerator : MonoBehaviour
{
    private List<Tuple<int, int>> GeneratedChunks { get; set; } = new();
    private Camera CameraMain { get; set; }
    public GameObject commonStoneBlock;
    public GameObject chunkFloor;
    private NavMeshGenerator NMG { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        CameraMain = Camera.main;
        NMG = GameObject.Find("NavMesh").GetComponent<NavMeshGenerator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckCameraPosition();
    }

    private void CheckCameraPosition()
    {
        Vector3 cameraPosition = CameraMain.transform.position;

        int ceiledX = (int)Math.Ceiling(cameraPosition.x / 10);
        int ceiledY = (int)Math.Ceiling(cameraPosition.y / 10);

        if (cameraPosition.x <= 0)
        {
            ceiledX--;
        }

        if(cameraPosition.y <= 0)
        {
            ceiledY--;
        }

        Tuple<int, int> cameraChunk = Tuple.Create(ceiledX, ceiledY);

        if (!GeneratedChunks.Contains(cameraChunk))
        {
            GeneratedChunks.Add(cameraChunk);

            StartCoroutine(GenerateChunk(ceiledX, ceiledY));
        }
    }

    IEnumerator GenerateChunk(int ceiledX, int ceiledY)
    {
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

        for (int i = 0; i < 10; i++)
        {
            totalX = originalTotalX;
            for (int j = 0; j < 10; j++)
            {
                Collider2D hitCollider = Physics2D.OverlapCircle(new Vector2(totalX, totalY), 0f);
                if (hitCollider == null)
                {
                    Instantiate(commonStoneBlock, new Vector3(totalX, totalY, 0), Quaternion.identity);
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

        GameObject newChunkFloor = Instantiate(chunkFloor, new Vector3(centerX, centerY, 0), Quaternion.identity);

        yield return new WaitForSeconds(0.01f);

        NMG.GenerateNavMesh();
    }
}
