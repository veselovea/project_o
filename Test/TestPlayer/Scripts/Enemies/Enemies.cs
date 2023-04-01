using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NamesOfEnemies
{
    BigOgr,
    Ogr
}

public abstract class Enemies : MonoBehaviour
{
    public abstract NamesOfEnemies EnemiesName { get; protected set; }
    public abstract GameObject Player { get; protected set; }

    public abstract int Health { get; protected set; }
    public abstract int Damage { get; protected set; }
    public abstract float VisibilityDistance { get; protected set; }
    public abstract float Speed { get; protected set; }
    public virtual bool IsCanAttack { get; protected set; } = true;

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
        Health -= Damage;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.GetComponent<Player>().TakeDamage(Damage);
        }
    }
}


