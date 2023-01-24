using UnityEngine;

public enum GameResources
{
    Wood,
    Stone
}

public abstract class Resource : MonoBehaviour
{
    public abstract GameResources Type { get; }
    public abstract float Health { get; protected set; }

    public virtual float GetResource(float damage)
    {
        if (damage > Health)
        {
            damage = damage - Health;
            Destroy(transform.gameObject);
        }
        else
            Health -= damage;
        return damage - damage * 0.2f;
    }
}
