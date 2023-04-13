using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScript : ResourceBlock
{
    public override int Durability { get; set; }
    public override ResourcesFromBlocks Type { get; set; }

    void Start()
    {
        Durability = 10;
        Type = ResourcesFromBlocks.Grass;
    }
}
