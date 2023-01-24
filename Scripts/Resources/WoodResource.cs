public class WoodResource : Resource
{
    public override GameResources Type => GameResources.Wood;

    public override float Health { get; protected set; }

    void Start()
    {
        Health = 200;
    }
}
