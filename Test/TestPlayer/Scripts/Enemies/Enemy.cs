using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public GameObject player;
    public GameObject effect;
    private Animator Anim;
    private Weapons weapon;

    //Specifications
    public int health;
    public float speed;
    private float visibilityDistance;

    //Stun
    private float stunTime;
    public float stStunTime;

    private void Awake()
    {
        Anim = GetComponentInParent<Animator>();
        weapon = GetComponentInChildren<Weapons>();
    }

    private void Update()
    {
        //AI
        visibilityDistance = Vector2.Distance(transform.position, player.transform.position);

        if(visibilityDistance < 10) 
        {
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        }

        //Attack
        if (visibilityDistance < 2)
        {
            weapon.Attack();
        }

        //Health
        if (health <= 0)
        {
            Destroy(gameObject);
        }

        //Stun
        if (stunTime <= 0)
        {
            speed = 1;
        }
        else
        {
            speed = 0;
            stunTime -= Time.deltaTime;
        }
    }

    public void TakeDamage(int Damage)
    {
        //Taking damage from player
        Instantiate(effect, transform.position, Quaternion.identity);
        health -= Damage;
        stunTime = stStunTime;
    }
}
