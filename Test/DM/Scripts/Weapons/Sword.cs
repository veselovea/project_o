using UnityEngine;

public class Sword : Weapons
{
    public override NamesOfWeapons WeaponName { get; } = NamesOfWeapons.Sword;
    public override int Damage { get; protected set; } = 50;
    public override float Radius { get; protected set; } = 50;
    public override int Speed { get; protected set; } = 5;
    public override Animator Anim { get; protected set; } 
    public override LayerMask Enemy { get; protected set; }

    private void Awake()
    {
        LayerMask.GetMask("Enemy");
    }
}