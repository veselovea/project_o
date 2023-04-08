using UnityEngine;

public class BigBat : Weapons
{
    public override NamesOfWeapons WeaponName { get; protected set; } = NamesOfWeapons.BigBat;
    public override Collider2D WeaponColliider { get; protected set; }
    public override int Damage { get; protected set; } = 100;
    public override float Speed { get; protected set; } = 3f;

    public void Start()
    {
        WeaponColliider = GetComponent<Collider2D>();
    }
}
