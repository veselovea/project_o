using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : ResourceBlock
{
    public override int Durability { get; set; }
    public override ResourcesFromBlocks Type { get; set; }

    void Start()
    {
        Durability = 100;
        Type = ResourcesFromBlocks.Stone;
    }
}
