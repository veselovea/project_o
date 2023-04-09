using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NamesOfCreatures
{
    Carl,
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

    //public GameObject player;

    public int Hl = 100;
    private Vector3 startPosition;
    private Weapons weapon;

    private CircleCollider2D circleCollider;
    private BoxCollider2D boxCollider;

    public void Start()
    {
        Player = GameObject.Find("Player");
        startPosition = this.transform.position;
    }

    private void Awake()
    {
        weapon = GetComponentInChildren<Weapons>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        //Health
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2d(Collider2D collision)
    {
        Collider2D circleCollision = collision.gameObject.GetComponent<Collider2D>();

        if (circleCollider.IsTouching(circleCollision))
        {
            transform.position = Vector2.MoveTowards(this.transform.position, Player.transform.position, Speed * Time.deltaTime);
        }
    }

    public void TakeDamage(int Damage)
    {
        //Taking damage
        Hl -= Damage;
    }
}
