using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NamesOfCrushers
{
    MiniGoblin,
}

public abstract class Crushers : MonoBehaviour
{
    public abstract GameObject Block { get; protected set; }
    public virtual Animator Anim { get; protected set; }

    public abstract NamesOfCrushers EnemiesName { get; protected set; }
    public abstract int Health { get; protected set; }
    public abstract float Speed { get; protected set; }
    public abstract float VisibilityDistance { get; protected set; }

    public virtual bool IsCanBeat { get; protected set; } = true;

    public Tools tool;
    public Vector3 startPosition;

    public void Start()
    {
        Block = GameObject.Find("HitBox");
        startPosition = this.transform.position;
    }
    public void Awake()
    {
        Anim = GetComponent<Animator>();
        tool = GetComponentInChildren<Tools>();
    }
    public void Update()
    {
        //AI
        VisibilityDistance = Vector2.Distance(transform.position, Block.transform.position);

        if (VisibilityDistance < 100)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, Block.transform.position - transform.right, Speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(this.transform.position, startPosition, Speed * Time.deltaTime);
        }

        //Attack
        if (VisibilityDistance < 2)
        {
            tool.Attack();
        }

        //Health
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int Damage)
    {
        Health -= Damage;
    }
}


