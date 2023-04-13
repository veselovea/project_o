using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassScript : ResourceBlock
{
    public override int Durability { get; set; }

    public override ResourcesFromStructure Type { get; set; }

    public override int �ount { get; set; }

    void Start()
    {
        Durability = 10;
        Type = ResourcesFromStructure.Grass;
        �ount = 1;
    }
}
