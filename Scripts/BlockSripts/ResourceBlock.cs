using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResDrop
{
    public ResourcesFromStructure Type;
    public int count;

    public ResDrop(ResourcesFromStructure type, int count)
    {
        Type = type;
        this.count = count;
    }
}

abstract public class ResourceBlock : MonoBehaviour
{
    public event Action<GameObject> OnBreakBlock;

    public abstract int Durability { get; set; }

    public abstract ResourcesFromStructure Type { get; set; }

    public abstract int Сount { get; set; }

    /// <summary>
    /// Наносит урон блоку.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>В случае, если у блока заканчивается прочность возвращает класс ResDrop</returns>

    public virtual ResDrop Break(int damage)
    {
        Durability -= damage;
        if (Durability < 0)
        {
            OnBreakBlock?.Invoke(gameObject);

            try
            {
                transform.GetComponent<Collider2D>().enabled = false;
                transform.parent.GetComponent<BlockSidesChanger>().CheckNeighbors();
                GameObject.Find("NavMesh").GetComponent<NavMeshGenerator>().GenerateNavMesh();
            }
            catch { }

            Destroy(transform.parent.gameObject);
            return new ResDrop(Type, Сount);
        }
        return null;
    }
}
