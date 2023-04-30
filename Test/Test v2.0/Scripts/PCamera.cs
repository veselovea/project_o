using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCamera : MonoBehaviour
{
    public Transform Player;
    private float x = 0f;
    private float y = 0f;

    private void LateUpdate()
    {
        Vector3 delta = Vector3.zero;

        //for X
        float deltaX = Player.position.x - transform.position.x;
        if (deltaX > x || deltaX < -x)
        {
            if (transform.position.x < Player.position.x)
            {
                delta.x = deltaX - x;
            }
            else
            {
                delta.x = deltaX + x;
            }
        }

        //for Y
        float deltaY = Player.position.y - transform.position.y;
        if (deltaY > y || deltaY < -y)
        {
            if (transform.position.y < Player.position.y)
            {
                delta.y = deltaY - y;
            }
            else
            {
                delta.y = deltaY + y;
            }
        }

        transform.position += new Vector3(delta.x, delta.y, 0);
    }
}
