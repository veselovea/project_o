using UnityEngine;

public class Sword : Weapons
{
    public override NamesOfWeapons WeaponName { get; protected set; } = NamesOfWeapons.Sword;
    public virtual Collider2D Coll { get; protected set; }
    public override int Damage { get; protected set; } = 10;
    public override float RangeAttack { get; protected set; } = 1;
    public override float Speed { get; protected set; } = 1;

    public void Start()
    {
        Coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            collider.GetComponent<Enemy>().TakeDamage(Damage);
        }
    }
}
