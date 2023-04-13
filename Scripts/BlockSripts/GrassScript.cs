using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScript : ResourceBlock
{
    public override int Durability { get; set; }

    public override ResourcesFromStructure Type { get; set; }

    public override int Ñount { get; set; }

    void Start()
    {
        Durability = 10;
        Type = ResourcesFromStructure.Grass;
        Ñount = 1;
    }
}
