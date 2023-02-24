using UnityEngine;

public class Sword : Weapons
{
    public override NamesOfWeapons WeaponName { get; protected set; } = NamesOfWeapons.Sword;
    public override int Damage { get; protected set; } = 10;
    public override float RangeAttack { get; protected set; } = 1;
    public override float Speed { get; protected set; } = 1;

}
