

/// <summary>
/// Класс представляюзий игровой объект сетевого взаимодействия между игроками.
/// Предназначен для взаимодействия между игроками в сетевой игре.
/// </summary>
public class GameNetworkObject
{
    public GameCommand Command { get; set; }
    public string CommandArgument { get; set; }
}

