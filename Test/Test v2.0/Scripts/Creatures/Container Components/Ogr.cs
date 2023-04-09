using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogr : Creatures
{
    public override NamesOfCreatures CreaturesName { get; protected set; } = NamesOfCreatures.Ogr;
    public override GameObject Player { get; protected set; }
    public Weapons Weapons { get; protected set; }
    public Vector3 moveDelta { get; protected set; }

    //Specifications
    public override int Health { get; protected set; } = 100;
    public override float Speed { get; protected set; } = 5;
    public override float VisibilityDistance { get; protected set; } = 0;

    private void Start()
    {
        Weapons = GetComponentInChildren<Weapons>();
    }
}
