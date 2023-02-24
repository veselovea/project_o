using System.Collections;
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

    public abstract int Damage { get; protected set; }
    public abstract float RangeAttack { get; protected set; }
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
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.TransformPoint(Vector3.zero), RangeAttack);

            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<Enemy>().TakeDamage(Damage);
            }
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