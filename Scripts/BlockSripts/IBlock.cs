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
    /// ������� ���� �����.
    /// </summary>
    /// <param name="damage"></param>
    /// <returns>���������� true ���� ���� ������.</returns>
    public bool DamageBlock(float damage)
    {
        Health -= damage;
        if (Health <= 0) return true;
        return false;
    }
}
