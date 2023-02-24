using UnityEngine;

public class Bat : Weapons
{
    public override NamesOfWeapons WeaponName { get; protected set; } = NamesOfWeapons.Bat;
    public override int Damage { get; protected set; } = 15;
    public override float RangeAttack { get; protected set; } = 1;
    public override float Speed { get; protected set; } = 2;

}
