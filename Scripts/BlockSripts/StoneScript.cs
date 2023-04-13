using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneScript : ResourceBlock
{
    public override int Durability { get; set; }

    public override ResourcesFromStructure Type { get; set; }

    public override int Ñount { get; set; }

    void Start()
    {
        Durability = 100;
        Type = ResourcesFromStructure.Stone;
        Ñount = 1;
    }
}
