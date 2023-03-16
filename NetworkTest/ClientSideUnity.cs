using System.Text;
using System.Threading.Tasks;

public class ClientSideUnity : UDPClientSide
{
    private NetworkHandlerLocalPlayer _localPlayer;
    private NetworkHandlerRemotePlayer _remotePlayer;

    public ClientSideUnity(string address, ushort port,
        NetworkHandlerLocalPlayer localPlayer, NetworkHandlerRemotePlayer remotePlayer)
        : base(address, port, 5126)
    {
        _localPlayer = localPlayer;
        _remotePlayer = remotePlayer;
    }

    public ClientSideUnity(string address, ushort port, ushort localPort,
        NetworkHandlerLocalPlayer localPlayer, NetworkHandlerRemotePlayer remotePlayer)
        : base(address, port, localPort)
    {
        _localPlayer = localPlayer;
        _remotePlayer = remotePlayer;
    }

    protected override Task ReceiveHandler(byte[] data)
    {
        return Task.Run(() =>
        {
            string text = Encoding.ASCII.GetString(data);
            GameNetworkPacket packet = Serializer.GetObject<GameNetworkPacket>(text);
            switch (packet.Command)
            {
                case GeneralCommand.PlayerInfo:
                    _localPlayer.PlayerInfo = packet.Player;
                    break;
                case GeneralCommand.ConnectedPlayersInfo:
                    PlayerInfo[] players = Serializer.GetObject<PlayerInfo[]>(packet.Data);
                    foreach (var item in players)
                        _remotePlayer.Born(item);
                    break;
                case GeneralCommand.GameCommand:
                    GameNetworkObject data = Serializer.GetObject<GameNetworkObject>(packet.Data);
                    switch (data.Command)
                    {
                        case GameCommand.Born:
                            _remotePlayer.Born(packet.Player);
                            break;
                        case GameCommand.Dead:
                            _remotePlayer.Dead(packet.Player);
                            break;
                        case GameCommand.Move:
                            _remotePlayer.Move(packet.Player, null);
                            break;
                        case GameCommand.Attack:
                            break;
                        default:
                            break;
                    }
                    break;
                case GeneralCommand.Connect:
                    if (packet.Type == NetworkObjectType.ConnectionToPoolResult)
                    {
                        _localPlayer.PlayerInfo = packet.Player;
                        _localPlayer.Connect();
                    }
                    else
                        _remotePlayer.Born(packet.Player);
                    break;
                case GeneralCommand.Disconnect:
                    _remotePlayer.Dead(packet.Player);
                    break;
                default:
                    break;
            }
        });
    }
}
