using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Eblock
{
    public string BlockName { get; set; }
    public Vector3 BlockPosition { get; set; }

    public Eblock(string blockName, Vector3 blockPosition)
    {
        this.BlockName = blockName;
        this.BlockPosition = blockPosition;
    }

    public Eblock()
    {
        this.BlockName = "";
        this.BlockPosition = Vector3.zero;
    }
}

public class BaseCore : MonoBehaviour
{
    private List<GameObject> blockList = new List<GameObject>();
    private List<Eblock> baseStructure = new List<Eblock>();

    public static event Action<Eblock[]> OnSaveFortress;

    void Start()
    {
        blockList.AddRange(Resources.LoadAll<GameObject>("Blocks"));
        NetworkDataReceive.OnLoadFortress += LoadFortress;
    }

    public void WriteDebug()
    {
        Debug.Log("baseStructure: " + baseStructure.Count);
    }

    public void AddBlock(string blockName, Vector3 blockPosition)
    {
        baseStructure.Add(new Eblock(blockName, blockPosition));
    }

    public void RemoveBlock(string blockName, Vector3 blockPosition)
    {
/*        Eblock eblockRemove = null;
        foreach (Eblock eBlock in baseStructure)
        {
            if (eBlock.BlockName == blockName && eBlock.BlockPosition == blockPosition)
            {
                eblockRemove = eBlock;
            }
        }
        if (eblockRemove != null)
        {
            baseStructure.Remove(eblockRemove);
        }*/

        baseStructure.RemoveAll(baseStructure => baseStructure.BlockName == blockName && baseStructure.BlockPosition == blockPosition);
    }

    public void SaveFortress(Eblock[] blocks) 
    {
        OnSaveFortress?.Invoke(blocks);
    }

    public void LoadFortress(Eblock[] blocks)
    {
        baseStructure.Clear();
        baseStructure.AddRange(blocks);

        foreach (Eblock block in baseStructure)
        {
            GameObject original = blockList.Where(blockList => blockList.gameObject.name == block.BlockName).First();

            Instantiate(original, block.BlockPosition, Quaternion.identity, transform);
        }
    }
}
