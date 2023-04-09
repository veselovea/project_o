using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float speed = 1f;
    private float distance = 5;
    private int Damage = 15;
    private LayerMask layer;

    public GameObject Player;

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.transform.position, distance, layer);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<Creatures>().TakeDamage(Damage);
            }
            Destroy(gameObject);
        }
        //transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
}
