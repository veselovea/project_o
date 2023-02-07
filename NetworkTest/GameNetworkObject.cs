internal enum GameCommands
{
    None,
    Connect,
    Disconnect,
    Move,
    Attack
}

/// <summary>
/// Класс представляюзий игровой объект сетевого взаимодействия между игроками.
/// Предназначен для взаимодействия между игроками в сетевой игре.
/// </summary>
internal class GameNetworkObject
{
    public string OwnerIP { get; set; }
    public string OwnerName { get; set; }
    public string GameCode { get; set; }
    public GameCommands Command { get; set; }
    public string CommandArgument { get; set; }

    public override string ToString()
    {
        string s = $"{OwnerIP}, {OwnerName}, {Command}";
        return s;
    }
}