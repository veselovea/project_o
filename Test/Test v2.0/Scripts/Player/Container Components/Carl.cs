using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Carl : Creatures
{
    public override NamesOfCreatures CreaturesName { get; protected set; } = NamesOfCreatures.Carl;
    public override string PlayerName { get; set; }
    public override Weapons weapon { get; protected set; }
    public Weapons Weapons { get; protected set; }

    private Transform weaponHolder;
    public Tools Tool { get; protected set; }
    public Vector3 moveDelta { get; protected set; }

    //Specifications
    public override int Health { get; protected set; } = 100;
    public override int CurrentHealth { get; protected set; }
    public override float Speed { get; protected set; } = 5;
    public override float VisibilityDistance { get; protected set; } = 0;

    private Rigidbody2D body;


    private void Start()
    {
        weaponHolder = transform.Find("WeaponHolder");
        Weapons = GetComponentInChildren<Weapons>();
        Tool = GetComponentInChildren<Tools>();
        body = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        //Attack
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0;

            if (Weapons != null)
            {
                Weapons.Attack();
                RotateWeaponTowardsAttackDirection();
            }
            else if(Tool != null)
            {
                Tool.Attack();
                RotateWeaponTowardsAttackDirection();
            }
        }
        else
        {
            weaponHolder.eulerAngles = Vector3.zero;
        }
    }

    private void RotateWeaponTowardsAttackDirection()
    {
        if(transform.localScale.x == 1)
        {
            weaponHolder.rotation = Quaternion.LookRotation(Vector3.forward, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            weaponHolder.eulerAngles += new Vector3(0, 0, 90);
        }
        else
        {
            weaponHolder.rotation = Quaternion.LookRotation(Vector3.forward, Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
            weaponHolder.eulerAngles += new Vector3(0, 0, -90);
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

        //body.AddForce(moveDelta * 5, ForceMode2D.Force);

        body.velocity = moveDelta * 5;

        if (moveDelta == Vector3.zero)
            body.velocity = Vector3.zero;
        //transform.Translate(Speed * moveDelta * Time.deltaTime);
    }
}