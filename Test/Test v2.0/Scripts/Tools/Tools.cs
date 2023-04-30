using System;
using System.Collections;
using System.Collections.Generic;
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

            for (int i = 0; i < resources.Count; i++)
            {
                if (resources[i] != null)
                {
                    resources[i].GetComponent<ResourceBlock>().Break(Damage);
                }
            }

            for (int i = 0; i < enemies.Count; i++)
            {
                if(enemies[i] != null)
                {
                    enemies[i].GetComponent<Enemies>().TakeDamage(Damage);
                }
            }
            StartCoroutine(Timeout());
        }
    }

    public IEnumerator Timeout()
    {
        IsCanBeat = false;
        yield return new WaitForSeconds(Speed);
        IsCanBeat = true;
    }

    private List<Collider2D> resources = new();

    private List<Collider2D> enemies = new();

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Block" && IsCanBeat == false)
        {
            resources.Add(collider);
        }

        if (transform.parent.parent.tag != "Enemy" && collider.gameObject.tag == "Enemy" && IsCanBeat == false)
        {
            enemies.Add(collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (resources.Contains(collider))
        {
            resources.Remove(collider);
        }

        if (enemies.Contains(collider))
        {
            enemies.Remove(collider);
        }
    }
}