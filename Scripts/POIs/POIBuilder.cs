using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POIBuilder : MonoBehaviour
{
    public List<Tuple<Vector3, GameObject>> Blocks { get; set; } = new();
    public List<Tuple<Vector3, GameObject>> Mobs { get; set; } = new();
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

        foreach (POIMobSpawner mobSpawner in transform.GetComponentsInChildren<POIMobSpawner>())
        {
            Mobs.Add(new Tuple<Vector3, GameObject>(mobSpawner.transform.position, mobSpawner.mobToSpawn));
            Destroy(mobSpawner.gameObject);
        }
    }

    public void DestroyBlueprints()
    {
        foreach (POIBlueprintBlock blueprint in transform.GetComponentsInChildren<POIBlueprintBlock>())
        {
            Destroy(blueprint.gameObject);
        }

        foreach (POIMobSpawner mobSpawner in transform.GetComponentsInChildren<POIMobSpawner>())
        {
            Destroy(mobSpawner.gameObject);
        }
    }

    public Tuple<List<Tuple<Vector3, GameObject>>, List<Tuple<Vector3, GameObject>>> BuildInChunk(Tuple<int, int> chunkNumber)
    {
        List<Tuple<Vector3, GameObject>> blocksToGenerate = new();
        List<Tuple<Vector3, GameObject>> mobsToGenerate = new();

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

            foreach (Tuple<Vector3, GameObject> mob in Mobs)
            {
                int ceiledX = (int)Math.Ceiling(mob.Item1.x / 10);
                int ceiledY = (int)Math.Ceiling(mob.Item1.y / 10);

                if (mob.Item1.x <= 0)
                {
                    ceiledX--;
                }

                if (mob.Item1.y <= 0)
                {
                    ceiledY--;
                }

                if (chunkNumber.Item1 == ceiledX && chunkNumber.Item2 == ceiledY)
                {
                    mobsToGenerate.Add(mob);
                }
            }

            foreach (Tuple<Vector3, GameObject> mob in mobsToGenerate)
            {
                Mobs.Remove(mob);
            }

            //if (Blocks.Count == 0)
            //{
            //    StartCoroutine(DelayedDestroy());
            //}
        }

        return Tuple.Create(blocksToGenerate, mobsToGenerate);
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(1f);

        Destroy(transform.gameObject);
    }
}
