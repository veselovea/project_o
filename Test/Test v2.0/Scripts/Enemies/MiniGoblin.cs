using UnityEngine;

public class MiniGoblin : Crushers
{
    public override GameObject Block { get; protected set; }

    public override NamesOfCrushers EnemiesName { get; protected set; } = NamesOfCrushers.MiniGoblin;
    public override int Health { get; protected set; } = 25;
    public override float Speed { get; protected set; } = 1;
    public override float VisibilityDistance { get; protected set; }
}


