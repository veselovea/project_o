/// <summary>
/// Основной класс который будет импользоваться для сетевого взаимодействия
/// </summary>
public class GameNetworkPacket
{
    public PlayerInfo Player { get; set; }
    public NetworkObjectType Type { get; set; }
    public GeneralCommand Command { get; set; }
    public string Data { get; set; }
}
