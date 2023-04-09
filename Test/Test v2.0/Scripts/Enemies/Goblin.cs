using UnityEngine;

public class Goblin : Enemies
{
    public override GameObject Player { get; protected set; }

    public override NamesOfEnemies EnemiesName { get; protected set; } = NamesOfEnemies.Goblin;
    public override int Health { get; protected set; } = 50;
    public override float Speed { get; protected set; } = 1;
    public override float VisibilityDistance { get; protected set; } = 10;
}
