using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    private NavMeshAgent Agent { get; set; }

    public void Start()
    {
        Player = GameObject.Find("Player");
        startPosition = this.transform.position;
        StartCoroutine(ActivateAgent());
    }
    public void Awake()
    {
        Anim = GetComponent<Animator>();
        weapon = GetComponentInChildren<Weapons>();
    }

    private IEnumerator ActivateAgent()
    {
        yield return new WaitForSeconds(1f);
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateUpAxis = false;
        Agent.updateRotation = false;
        Agent.enabled = true;
    }

    bool isEnemyFound = false;

    bool isDestinationSetOnCD = false;
    private IEnumerator Cooldown()
    {
        isDestinationSetOnCD = true;
        yield return new WaitForSeconds(1f);
        isDestinationSetOnCD = false;
    }

    public void Update()
    {
        //AI
        VisibilityDistance = Vector2.Distance(transform.position, Player.transform.position);
        if (Agent != null && Agent.enabled == true)
        {


            if (VisibilityDistance < 25)
            {
                if (isEnemyFound == false)
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.transform.position - transform.position, Vector2.Distance(transform.position, Player.transform.position), LayerMask.GetMask("SolidBlock"));

                    if (hit.collider == null)
                    {
                        isEnemyFound = true;
                    }
                }
                else
                {
                    if (isDestinationSetOnCD == false)
                    {
                        Agent.SetDestination(Player.transform.position);
                        StartCoroutine(Cooldown());
                    }
                    //transform.position = Vector2.MoveTowards(this.transform.position, Player.transform.position, Speed * Time.deltaTime);
                }
            }
            else
            {
                isEnemyFound = false;

                if (Vector3.Distance(startPosition, transform.position) > 1)
                {
                    Agent.SetDestination(startPosition);
                }
                //transform.position = Vector2.MoveTowards(this.transform.position, startPosition, Speed * Time.deltaTime);
            }
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


