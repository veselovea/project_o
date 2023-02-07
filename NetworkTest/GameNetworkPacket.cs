internal enum NetworkObjectType
{
    GameNetworkObject,
    ConnectionToPoolResult
}

/// <summary>
/// Основной класс который будет импользоваться для сетевого взаимодействия
/// </summary>
internal class GameNetworkPacket
{
    public NetworkObjectType Type { get; set; }
    public string Data { get; set; }
}