using System;
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

abstract public class ResourceBlock : MonoBehaviour
{
    public event Action<GameObject> OnBreakBlock;

    public abstract int Durability { get; set; }

    public abstract ResourcesFromBlocks Type { get; set; }

    public virtual int Break(int damage)
    {
        Durability -= damage;
        if (Durability < 0)
        {
            OnBreakBlock?.Invoke(gameObject);
            return 1;
        }
        return 0;
    }
}
