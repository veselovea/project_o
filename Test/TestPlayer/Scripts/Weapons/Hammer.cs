using UnityEngine;

public class Hammer : Weapons
{
    public override NamesOfWeapons WeaponName { get; protected set; } = NamesOfWeapons.Hammer;
    public override int Damage { get; protected set; } = 35;
    public override float RangeAttack { get; protected set; } = 2;
    public override float Speed { get; protected set; } = 5;

}
