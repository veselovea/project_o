using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IBlock
{
    public int Id { get; set; }
    public float Health { get; set; }
    public string Name { get; set; }

    /// <summary>
    /// Наносит урон блоку.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>Возвращает true если блок сломан.</returns>
    public bool DamageBlock(float damage)
    {
        Health -= damage;
        if (Health <= 0) return true;
        return false;
    }
}
