using UnityEngine;

public class Bat : Weapons
{
    public override NamesOfWeapons WeaponName { get; protected set; } = NamesOfWeapons.Bat;
    public virtual Collider2D WeaponColliider { get; protected set; }
    public override int Damage { get; protected set; } = 25;
    public override float Speed { get; protected set; } = 2;

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

        if (collider.gameObject.tag == "Player" && IsCanAttack == false)
        {
            collider.GetComponent<Player>().TakeDamage(Damage);
        }
    }
}
