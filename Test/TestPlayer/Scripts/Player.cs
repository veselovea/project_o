using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private Weapons weapon;

    public float speed;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        weapon = GetComponentInChildren<Weapons>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            weapon.Attack();
        }
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        moveDelta = new Vector3(x, y, 0);

        if (moveDelta.x > 0)
            transform.localScale = Vector3.one;
        else if (moveDelta.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        transform.Translate(speed * moveDelta * Time.deltaTime);
    }
}
