using UnityEngine;

public class Sword : Weapons
{
    public override NamesOfWeapons WeaponName { get; protected set; } = NamesOfWeapons.Sword;
    public override Collider2D WeaponColliider { get; protected set; }
    public override int Damage { get; protected set; } = 50;
    public override float Speed { get; protected set; } = 0.5f;

    public void Start()
    {
        WeaponColliider = GetComponent<Collider2D>();
    }
}
