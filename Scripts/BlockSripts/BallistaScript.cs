using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BallistaScript : MonoBehaviour
{
    public string Owner;
    GameObject circle;
    GameObject player;

    void Start()
    {
        Owner = null;
        circle = GameObject.Find("ballista").transform.Find("Tower").gameObject;
        player = GameObject.Find("Player");
    }

    void Update()
    {
        Vector3 vector = Vector3.RotateTowards(circle.transform.position, (player.transform.position - circle.transform.position), 1,  0); 
        circle.transform.rotation = Quaternion.LookRotation(vector);
    }
}
