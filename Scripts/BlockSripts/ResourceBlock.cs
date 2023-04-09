using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ResourcesFromBlocks
{
    Door,
    Stone,
    Grass
}

public class ResourceBlock : MonoBehaviour
{
    public int Durability { get; set; }

    public ResourcesFromBlocks Type { get; set; }

    public int Break(int damage)
    {
        Durability -= damage;
        if (Durability < 0)
        {
            Destroy(transform.parent.gameObject);
            return 1;
        }
        return 0;
    }
}
