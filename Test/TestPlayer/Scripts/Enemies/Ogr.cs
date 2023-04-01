using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogr : Enemies
{
    public override NamesOfEnemies EnemiesName { get; protected set; } = NamesOfEnemies.Ogr;
    public override GameObject Player { get; protected set; }

    public override int Health { get; protected set; } = 100;
    public override int Damage { get; protected set; } = 15;
    public override float VisibilityDistance { get; protected set; } = 5;
    public override float Speed { get; protected set; } = 1;
}
