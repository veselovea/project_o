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
    public Vector3 moveDelta { get; protected set; }

    //Specifications
    public override int Health { get; protected set; } = 100;
    public override int CurrentHealth { get; protected set; }
    public override float Speed { get; protected set; } = 5;
    public override float VisibilityDistance { get; protected set; } = 0;


    private void Start()
    {
        Weapons = GetComponentInChildren<Weapons>();
    }

    public void Update()
    {
        //Attack
        if (Input.GetMouseButton(0))
        {
            Weapons.Attack();
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

        transform.Translate(Speed * moveDelta * Time.deltaTime);
    }
}