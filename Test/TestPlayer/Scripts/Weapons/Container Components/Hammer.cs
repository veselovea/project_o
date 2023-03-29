using UnityEngine;

public class Hammer : Weapons
{
    public override NamesOfWeapons WeaponName { get; protected set; } = NamesOfWeapons.Hammer;
    public virtual Collider2D WeaponColliider { get; protected set; }
    public override int Damage { get; protected set; } = 35;
    public override float Speed { get; protected set; } = 5;

    public void Start()
    {
        WeaponColliider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy" && IsCanAttack == false)
        {
            collider.GetComponent<Enemy>().TakeDamage(Damage);
        }
    }
}
