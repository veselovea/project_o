using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BaseCoreMultiplayer : MonoBehaviour
{
    private List<GameObject> blockList = new List<GameObject>();
    private List<Eblock> baseStructure = new List<Eblock>();

    // Start is called before the first frame update
    void Start()
    {
        blockList.AddRange(Resources.LoadAll<GameObject>("Blocks"));
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
