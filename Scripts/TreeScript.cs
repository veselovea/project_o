using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour
{
    public int health = 100;

    void FixedUpdate()
    {
        if (health < 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
