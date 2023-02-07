using UnityEngine;

public enum NamesOfWeapons
{
    Sword,
    Hammer,
    Bat
}

public abstract class Weapons : MonoBehaviour
{
    public abstract NamesOfWeapons WeaponName { get; }
    public abstract int Damage { get; protected set; }
    public abstract float Radius { get; protected set; }
    public abstract int Speed { get; protected set; }
    public abstract Animator Anim { get; protected set; }
    public abstract LayerMask Enemy { get; protected set; }

    public virtual void Attack()
    {
        Anim.SetTrigger("attack");
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Attack();
        }
    }
}