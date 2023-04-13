using System;
using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public enum NamesOfTools
{
    Axe,
}

public abstract class Tools : MonoBehaviour
{
    public abstract NamesOfTools ToolsName { get; protected set; }
    public virtual Animator Anim { get; protected set; }
    public virtual Collider2D ToolsColliider { get; protected set; }

    public abstract int Damage { get; protected set; }
    public abstract float Speed { get; protected set; }
    public virtual bool IsCanBeat { get; protected set; } = true;

    private void Awake()
    {
        Anim = GetComponentInParent<Animator>();
    }

    public virtual void Attack()
    {

        if (IsCanBeat)
        {
            Anim.SetTrigger("attack");
            StartCoroutine(Timeout());
        }
    }

    public IEnumerator Timeout()
    {
        IsCanBeat = false;
        yield return new WaitForSeconds(Speed);
        IsCanBeat = true;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Block" && IsCanBeat == false)
        {
            collider.GetComponent<ResourceBlock>().Break(Damage);
        }
    }
}