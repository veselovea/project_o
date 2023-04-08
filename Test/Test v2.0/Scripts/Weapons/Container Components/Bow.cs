using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bow : MonoBehaviour
{
    public Arrow arrow;

    private void Awake()
    {
        arrow = GetComponentInChildren<Arrow>();
    }

    public void Shooting()
    {
        Instantiate(arrow, transform.position, transform.rotation);
    }
}
