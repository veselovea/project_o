using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public enum NamesOfCreatures
{
    Carl,
    CarlNet,
}

public abstract class Creatures : MonoBehaviour
{
    public abstract NamesOfCreatures CreaturesName { get; protected set; }
    public abstract Weapons weapon { get; protected set; }
    public abstract string PlayerName { get; set; }

    //Specifications
    public abstract int Health { get; protected set; }
    public abstract int CurrentHealth { get; protected set; }
    public abstract float Speed { get; protected set; }
    public abstract float VisibilityDistance { get; protected set; }

    //Net
    public Func<bool, float, string, Task> OnPlayerAttack { get; set; }
    public event Action<Hitted> OnAttackPlayer;

    public event Action OnPlayerDead;
    private bool IsDead = false;

    private void Awake()
    {
        weapon = GetComponentInChildren<Weapons>();
        if (weapon != null)
        {
            weapon.OnAttackPlayer += AttackCallback;
        }
    }

    public void AttackCallback(Hitted hit)
    {
        OnAttackPlayer?.Invoke(hit);
    }

    public void TakeDamage(int Damage)
    {
        if (Damage >= Health)
        {
            OnPlayerDead?.Invoke();
            IsDead = true;
        }
        CurrentHealth -= Damage;
    }

    public void ResetHealth()
    {
        if (IsDead)
            CurrentHealth = Health;
    }
}
public class Hitted
{
    public bool IsHit { get; set; }
    public float Damage { get; set; }
    public string Recipient { get; set; }
}
