using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIBuilder : MonoBehaviour
{
    public List<Tuple<Vector3, GameObject>> Blocks { get; set; } = new();
    public List<Tuple<int, int>> AffectedChunks { get; set; } = new();

    // Start is called before the first frame update
    //void Start()
    //{
    //    foreach (POIBlueprintBlock blueprint in transform.GetComponentsInChildren<POIBlueprintBlock>())
    //    {
    //        Blocks.Add(new Tuple<Vector3, GameObject>(blueprint.transform.position, blueprint.desiredBlock));
    //        Destroy(blueprint.gameObject);
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DetermineBlocks()
    {
        foreach (POIBlueprintBlock blueprint in transform.GetComponentsInChildren<POIBlueprintBlock>())
        {
            Blocks.Add(new Tuple<Vector3, GameObject>(blueprint.transform.position, blueprint.desiredBlock));
            Destroy(blueprint.gameObject);
        }
    }

    public void DestroyBlueprints()
    {
        foreach (POIBlueprintBlock blueprint in transform.GetComponentsInChildren<POIBlueprintBlock>())
        {
            Destroy(blueprint.gameObject);
        }
    }

    public List<Tuple<Vector3, GameObject>> BuildInChunk(Tuple<int, int> chunkNumber)
    {
        List<Tuple<Vector3, GameObject>> blocksToGenerate = new();

        if (!AffectedChunks.Contains(chunkNumber))
        {
            AffectedChunks.Add(chunkNumber);

            foreach (Tuple<Vector3, GameObject> block in Blocks)
            {
                int ceiledX = (int)Math.Ceiling(block.Item1.x / 10);
                int ceiledY = (int)Math.Ceiling(block.Item1.y / 10);

                if (block.Item1.x <= 0)
                {
                    ceiledX--;
                }

                if (block.Item1.y <= 0)
                {
                    ceiledY--;
                }

                if (chunkNumber.Item1 == ceiledX && chunkNumber.Item2 == ceiledY)
                {
                    blocksToGenerate.Add(block);
                }
            }

            foreach (Tuple<Vector3, GameObject> block in blocksToGenerate)
            {
                Blocks.Remove(block);
            }

            //if (Blocks.Count == 0)
            //{
            //    StartCoroutine(DelayedDestroy());
            //}
        }

        return blocksToGenerate;
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(1f);

        Destroy(transform.gameObject);
    }
}
