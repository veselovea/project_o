internal enum GameEvents
{
    None,
    Join,
    Leave,
    GameCommand
}

internal enum GameCommands
{
    None,
    Move,
    Attack
}

internal class GameNetworkObject
{
    public string OwnerIP { get; set; }
    public string OwnerName { get; set; }
    public GameEvents Event { get; set; }
    public GameCommands Command { get; set; }
    public string CommandArgument { get; set; }

    public override string ToString()
    {
        string s = $"{OwnerIP}, {OwnerName}, {Event}, {Command}";
        return s;
    }
}
