using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private Weapons weapon;

    //Specifications
    public int health;
    public float speed;

    //Stun
    private float stunTime;
    public float stStunTime;

    private void Start()
    {
        weapon = GetComponentInChildren<Weapons>();
    }

    void Update()
    {
        //Attack
        if (Input.GetMouseButton(0))
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
            speed = 5;
        }
        else
        {
            speed = 0;
            stunTime -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        //Move and rotate
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveDelta = new Vector3(x, y, 0);

        if (moveDelta.x > 0)
            transform.localScale = Vector3.one;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 0);

        transform.Translate(speed * moveDelta * Time.deltaTime);
    }

    public void TakeDamage(int Damage)
    {
        //Taking damage from enemies
        health -= Damage;
        stunTime = stStunTime;
    }
}
