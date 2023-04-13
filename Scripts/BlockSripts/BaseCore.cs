using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Когда-то он был красивым и успешный "EnumBlock", теперь он просто "Eblock"
/// </summary>

public class Eblock
{
    public string BlockName { get; set; }
    public Vector3 BlockPosition { get; set; }

    public Eblock(string blockName, Vector3 blockPosition)
    {
        this.BlockName = blockName;
        this.BlockPosition = blockPosition;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SaveFortress(baseStructure.ToArray());
    }

    public void AddBlock(string blockName, Vector3 blockPosition)
    {
        baseStructure.Add(new Eblock(blockName, blockPosition));
    }

    public void RemoveBlock(string blockName, Vector3 blockPosition)
    {
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
