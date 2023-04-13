using UnityEngine;

public class RemotePlayerScript : MonoBehaviour, IRemotePlayer
{
    private int Health = 100;
    private Weapons _weapon;

    public void Attack()
        => _weapon?.Attack();

    public void SetWeapon(Weapons weapons)
    {
        if (_weapon is null)
            _weapon = weapons;
    }

    public bool TakeDamage(int value)
    {
        Health -= value;
        if (Health <= 0)
            return true;
        return false;
    }

    public void ResetHealth()
    {
        if (Health <= 0)
            Health = 100;
    }
}