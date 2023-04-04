using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkHandlerLocalPlayer : ExecuteTasksInMainThread, ILocalPlayer
{
    private ClientSideUnity _client;

    private GameObject _playerPrefub;
    private PlayerInfo _playerInfo;

    public NetworkHandlerLocalPlayer(GameObject playerPrefub, string playerName)
    {
        _playerPrefub = playerPrefub;
        _playerInfo = new PlayerInfo() { Name = playerName };
    }

    public PlayerInfo PlayerInfo { get => _playerInfo; set => _playerInfo = value; }
    public bool PlayerInGame => _playerInfo.GameCode is not null;
    public bool CanConnectToGame => _playerInfo.IP is not null;
    public ClientSideUnity Client { set { if (_client is null) _client = value; } }


    public PlayerTransform GetPlayerTransform()
    {
        PlayerTransform transform = new PlayerTransform()
        {
            PositionX = _playerPrefub.transform.position.x,
            PositionY = _playerPrefub.transform.position.y,
            PositionZ = _playerPrefub.transform.position.z,
            RotationX = _playerPrefub.transform.rotation.x,
            RotationY = _playerPrefub.transform.rotation.y,
            RotationZ = _playerPrefub.transform.rotation.z
        };
        return transform;
    }

    public async Task SendPlayerPosition()
    {
        PlayerTransform transform = GetPlayerTransform();
        GameNetworkObject networkObject = new GameNetworkObject()
        {
            Command = GameCommand.Move,
            CommandArgument = Serializer.GetJson(transform)
        };
        GameNetworkPacket packet = new GameNetworkPacket()
        {
            Player = _playerInfo,
            Command = GeneralCommand.GameCommand,
            Type = NetworkObjectType.GameNetworkObject,
            Data = Serializer.GetJson(networkObject)
        };
        string json = Serializer.GetJson(packet);
        byte[] buffer = Encoding.ASCII.GetBytes(json);
        await _client.SendAsync(buffer);
    }

    public async Task Attack(bool isHit, float damage, string hittedPlayerName)
    {
        PlayerTransform transform = GetPlayerTransform();
        GameNetworkObject networkObject = new GameNetworkObject()
        {
            Command = GameCommand.Attack,
            CommandArgument = Serializer.GetJson(transform)
        };
        if (isHit)
        {
            PlayerAttack attack = new PlayerAttack
            {
                Damage = damage,
                PlayerName = hittedPlayerName,
                Transform = transform
            };
            networkObject.Command = GameCommand.HittedAttack;
            networkObject.CommandArgument = Serializer.GetJson(attack);
        }
        GameNetworkPacket packet = new GameNetworkPacket()
        {
            Player = _playerInfo,
            Command = GeneralCommand.GameCommand,
            Type = NetworkObjectType.GameNetworkObject,
            Data = Serializer.GetJson(networkObject)
        };
        string json = Serializer.GetJson(packet);
        byte[] buffer = Encoding.ASCII.GetBytes(json);
        await _client.SendAsync(buffer);
    }

    public async Task UpdatePlayerInfo()
    {
        GameNetworkPacket packet = new GameNetworkPacket
        {
            Player = _playerInfo,
            Command = GeneralCommand.PlayerInfo,
            Type = NetworkObjectType.None,
            Data = null
        };
        string json = Serializer.GetJson(packet);
        byte[] buffer = Encoding.ASCII.GetBytes(json);
        await _client.SendAsync(buffer);
    }

    public async Task Connect(string code)
    {
        if (_playerInfo.IP is null)
            return;
        GameNetworkPacket packet = new GameNetworkPacket()
        {
            Player = _playerInfo,
            Command = GeneralCommand.Connect,
            Type = NetworkObjectType.None,
            Data = code
        };
        string json = Serializer.GetJson(packet);
        byte[] buffer = Encoding.ASCII.GetBytes(json);
        await _client.SendAsync(buffer);
    }

    public async Task Disconnect()
    {
        GameNetworkPacket packet = new GameNetworkPacket()
        {
            Player = _playerInfo,
            Command = GeneralCommand.Disconnect,
            Type = NetworkObjectType.None,
            Data = null
        };
        string json = Serializer.GetJson(packet);
        byte[] buffer = Encoding.ASCII.GetBytes(json);
        await _client.SendAsync(buffer);
    }

    public void Connected()
    {
        ExecutableAction action = new ExecutableAction();
        action.Execute += () =>
        {
            _playerPrefub = GameObject.Instantiate(_playerPrefub);
            _playerPrefub.name = _playerInfo.Name;
        };
        base.AddTaskToQueue(action);
    }
}