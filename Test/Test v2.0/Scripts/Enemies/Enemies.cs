using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NamesOfEnemies
{
    Goblin,
    BigGoblin,
    Skeleton
}

public abstract class Enemies : MonoBehaviour
{
    public abstract GameObject Player { get; protected set; }
    public virtual Animator Anim { get; protected set; }

    public abstract NamesOfEnemies EnemiesName { get; protected set; }
    public abstract int Health { get; protected set; }
    public abstract float Speed { get; protected set; }
    public abstract float VisibilityDistance { get; protected set; }

    public virtual bool IsCanAttack { get; protected set; } = true;

    public Weapons weapon;
    public Vector3 startPosition;

    public void Start()
    {
        Player = GameObject.Find("Player");
        startPosition = this.transform.position;
    }
    public void Awake()
    {
        Anim = GetComponent<Animator>();
        weapon = GetComponentInChildren<Weapons>();
    }
    public void Update()
    {
        //AI
        VisibilityDistance = Vector2.Distance(transform.position, Player.transform.position);

        if (VisibilityDistance < 25)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, Player.transform.position, Speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(this.transform.position, startPosition, Speed * Time.deltaTime);
        }

        //Attack
        if (VisibilityDistance < 4)
        {
            weapon.Attack();
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


