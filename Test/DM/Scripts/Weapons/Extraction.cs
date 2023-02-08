using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extraction : MonoBehaviour
{
    public Animator Anim;
    public Transform AttaclPos;

    private float timeExtraction;
    public float startTimeExtraction;

    public LayerMask wood;
    public float RangeAttack;
    public int Damage;


    private void Update()
    {
        if (timeExtraction <= 0)
        {
            timeExtraction = startTimeExtraction;

            if (Input.GetMouseButton(1))
            {
                Anim.SetTrigger("attack");
                Collider2D[] woods = Physics2D.OverlapCircleAll(AttaclPos.position, RangeAttack, wood);
                for (int i = 0; i < woods.Length; i++)
                {
                    woods[i].GetComponent<Wood>().TakeDamage(Damage);
                }
            }
        }
        else
        {
            timeExtraction -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(AttaclPos.position, RangeAttack);
    }
}
