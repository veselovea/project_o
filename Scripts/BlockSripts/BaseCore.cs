using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Eblock
{
    public string blockName;
    public Vector3 blockPosition;

    public Eblock(string blockName, Vector3 blockPosition)
    {
        this.blockName = blockName;
        this.blockPosition = blockPosition;
    }

    public Eblock()
    {
        this.blockName = "";
        this.blockPosition = Vector3.zero;
    }
}

public class BaseCore : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> blockList = new List<GameObject>();
    private List<Eblock> baseStructure = new List<Eblock>();

    void Start()
    {
        blockList.AddRange(Resources.LoadAll<GameObject>("Blocks"));
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
        Eblock eblockRemove = null;
        foreach (Eblock eBlock in baseStructure)
        {
            if (eBlock.blockName == blockName && eBlock.blockPosition == blockPosition)
            {
                eblockRemove = eBlock;
            }
        }
        if (baseStructure.Remove(eblockRemove) != null)
        {
            baseStructure.Remove(eblockRemove);
        }
    }

    public void LoadBase(List<Eblock> baseStructure)
    {
        baseStructure.Clear();
        baseStructure.AddRange(baseStructure);
    }

    public List<Eblock> GetBase()
    {
        return baseStructure;
    }
}
