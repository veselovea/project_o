using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum NamesOfWeapons
{
    Sword,
    Hammer,
    Bat
}

public abstract class Weapons : MonoBehaviour
{
    public abstract NamesOfWeapons WeaponName { get; protected set; }
    public virtual Animator Anim { get; protected set; }
    public virtual Collider2D WeaponColliider { get; protected set; }

    public abstract int Damage { get; protected set; }
    public abstract float Speed { get; protected set; }
    public virtual bool IsCanAttack { get; protected set; } = true;

    private void Awake() 
    {
        Anim = GetComponentInParent<Animator>();
    }

    public virtual void Attack()
    {
        if (IsCanAttack)
        {
            Anim.SetTrigger("attack");
            StartCoroutine(Timeout());
        }
    }

    private IEnumerator Timeout()
    {
        IsCanAttack = false;
        yield return new WaitForSeconds(Speed);
        IsCanAttack = true;
    }
}