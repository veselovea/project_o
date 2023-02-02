using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIBuilder : MonoBehaviour
{
    private List<Tuple<Vector3, GameObject>> blocks = new();
    public List<Tuple<int, int>> AffectedChunks { get; set; } = new();
    // Start is called before the first frame update
    void Start()
    {
        foreach (POIBlueprintBlock blueprint in transform.GetComponentsInChildren<POIBlueprintBlock>())
        {
            blocks.Add(new Tuple<Vector3, GameObject>(blueprint.transform.position, blueprint.desiredBlock));
            Destroy(blueprint.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Tuple<Vector3, GameObject>> BuildInChunk(Tuple<int, int> chunkNumber)
    {
        List<Tuple<Vector3, GameObject>> blocksToGenerate = new();

        if (!AffectedChunks.Contains(chunkNumber))
        {
            AffectedChunks.Add(chunkNumber);

            foreach (Tuple<Vector3, GameObject> block in blocks)
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
                blocks.Remove(block);
            }

            if (blocks.Count == 0)
            {
                StartCoroutine(DelayedDestroy());
            }
        }

        return blocksToGenerate;
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(1f);

        Destroy(transform.gameObject);
    }
}
