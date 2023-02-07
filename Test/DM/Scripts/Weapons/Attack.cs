using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Animator Anim;
    public Transform AttaclPos;

    private float timeAttack;
    public float startTimeAttack;

    public LayerMask enemy;
    public float RangeAttack;
    public int Damage;


    private void Update()
    {
        if (timeAttack <= 0)
        {
            timeAttack = startTimeAttack;

            if (Input.GetMouseButton(0))
            {
                Anim.SetTrigger("attack");
                Collider2D[] enemies = Physics2D.OverlapCircleAll(AttaclPos.position, RangeAttack, enemy);
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].GetComponent<Enemy>().TakeDamage(Damage);
                }
            }
        }
        else
        {
            timeAttack -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(AttaclPos.position, RangeAttack);
    }
}
