using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NamesOfCreatures
{
    Player,
    BigOgr,
    Ogr
}

public abstract class Creatures : MonoBehaviour
{
    public abstract NamesOfCreatures CreaturesName { get; protected set; }
    public abstract GameObject Player { get; protected set; }

    public abstract int Health { get; protected set; }
    public abstract float Speed { get; protected set; }
    public abstract float VisibilityDistance { get; protected set; }

    public void Start()
    {
        GameObject.Find("Player");
    }

    private void Update()
    {
        //AI
        VisibilityDistance = Vector2.Distance(transform.position, Player.transform.position);

        if (VisibilityDistance < 10)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, Player.transform.position, Speed * Time.deltaTime);
        }

        //Health
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(int Damage)
    {
        //Taking damage
        Health -= Damage;
    }
}
