using System;
using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public enum NamesOfWeapons
{
    Sword,
    BigBat,
    Bow
}

public abstract class Weapons : MonoBehaviour
{
    public abstract NamesOfWeapons WeaponName { get; protected set; }
    public virtual Animator Anim { get; protected set; }
    public virtual Collider2D WeaponColliider { get; protected set; }

    public abstract int Damage { get; protected set; }
    public abstract float Speed { get; protected set; }
    public virtual bool IsCanAttack { get; protected set; } = true;

    public event Action<Hitted> OnAttackPlayer;

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

    public IEnumerator Timeout()
    {
        IsCanAttack = false;
        yield return new WaitForSeconds(Speed);
        IsCanAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Hitted hit = new Hitted();
        hit.IsHit = false;

        if (true)
        {
            hit.IsHit = true;
            hit.Damage = Damage;
            hit.Recipient = "QWE";
        }
        OnAttackPlayer?.Invoke(hit);

        if (collider.gameObject.tag == "Enemy" && IsCanAttack == false)
        {
            collider.GetComponent<Enemies>().TakeDamage(Damage);
        }

        if (collider.gameObject.tag == "Player" && IsCanAttack == false)
        {
            collider.GetComponent<Creatures>().TakeDamage(Damage);
        }
    }
}