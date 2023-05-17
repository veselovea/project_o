//using UnityEditor;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public GameObject Player;
    private float VisibilityDistance;
    private Bow bow;

    private float Speed = 5f;

    private void Awake()
    {
        bow = GetComponentInChildren<Bow>();
    }

    private void Update()
    {
        //AI
        VisibilityDistance = Vector2.Distance(transform.position, Player.transform.position);

        //Attack
        if (VisibilityDistance < 5)
        {
            bow.Shooting();
        }
    }
}
