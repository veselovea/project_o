﻿using System.Threading.Tasks;

public class ClientSideUnity : ClientSide
{
    private NetworkHandlerLocalPlayer _localPlayer;
    private NetworkHandlerRemotePlayer _remotePlayer;

    public ClientSideUnity(string address, ushort port,
        NetworkHandlerLocalPlayer localPlayer, NetworkHandlerRemotePlayer remotePlayer)
        : base(address, port)
    {
        _localPlayer = localPlayer;
        _remotePlayer = remotePlayer;
    }

    protected override Task ReceiveHandler(string text)
    {
        return Task.Run(() =>
        {
            GameNetworkPacket packet = Serializer.GetObject<GameNetworkPacket>(text);
            switch (packet.Command)
            {
                case GeneralCommand.PlayerInfo:
                    _localPlayer.UpdatePlayerInfo(packet.Player);
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
                        _localPlayer.UpdatePlayerInfo(packet.Player);
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