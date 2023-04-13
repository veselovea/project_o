using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResDrop
{
    public ResourcesFromStructure Type;
    public int count;
}

abstract public class ResourceBlock : MonoBehaviour
{
    public event Action<GameObject> OnBreakBlock;

    public abstract int Durability { get; set; }

    public abstract ResourcesFromStructure Type { get; set; }

    public abstract int count { get; set; }

    public virtual ResDrop Break(int damage)
    {
        Durability -= damage;
        if (Durability < 0)
        {
            ResDrop resDrop = new ResDrop();
            resDrop.Type = Type;
            resDrop.count = count;
            OnBreakBlock?.Invoke(gameObject);
            Destroy(gameObject);
            return resDrop;
        }
        return null;
    }
}
