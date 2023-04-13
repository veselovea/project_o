using UnityEngine;

public class RemotePlayerScript : Creatures, IRemotePlayer
{
    public override NamesOfCreatures CreaturesName { get; protected set; }
    public override GameObject Player { get; protected set; }
    public override int Health { get; protected set; }
    public override float Speed { get; protected set; }
    public override float VisibilityDistance { get; protected set; }

    void Awake()
    {
        CreaturesName = NamesOfCreatures.Carl;
        
    }

    public bool Attack(Vector3 direction)
    {
        return false;
    }
}