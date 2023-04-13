using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NetworkHandlerLocalPlayer : ExecuteTasksInMainThread, ILocalPlayer, IDBREceiveHandler
{
    private ClientSideUnity _client;

    private GameObject _playerPrefub;
    private PlayerInfo _playerInfo;

    private BaseInitializer _baseInitializer;
    private DataClientSide _dataSide;

    public NetworkHandlerLocalPlayer(GameObject playerPrefub, string playerName, BaseInitializer baseInitializer)
    {
        _playerPrefub = playerPrefub;
        _playerInfo = new PlayerInfo() { Name = playerName };
        _baseInitializer = baseInitializer;
        _dataSide = new DataClientSide(this);
        _dataSide.Start();
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

    public async Task Attack(bool isHit, int damage, string hittedPlayerName)
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

        DataNetworkPacket packet = new DataNetworkPacket()
        {
            Command = DataCommand.GetFortress,
            Argument = _playerInfo.Name
        };
        byte[] buffer = Encoding.ASCII.GetBytes(
            Serializer.GetJson(packet));
        _dataSide.Send(buffer);
    }

    public void LoadFortress(FortressData data)
    {
        Eblock[] eblocks = new Eblock[data.Blocks.Length];
        for (int i = 0; i < eblocks.Length; i++)
        {
            string[] temp = data.Blocks[i].Position.Split(',');
            float x = Convert.ToSingle(temp[0].Replace('.', ','));
            float y = Convert.ToSingle(temp[1].Replace('.', ','));
            float z = Convert.ToSingle(temp[2].Replace('.', ','));
            Vector3 position = new Vector3(x, y, z);
            eblocks[i] = new Eblock(data.Blocks[i].Name, position);
        }
        PlayerBaseObject playerBase = new PlayerBaseObject
        {
            Player = _playerPrefub,
            PlayerBaseBlocks = eblocks
        };
        _dataSide.Stop();
        _baseInitializer.SetupBase(playerBase);
    }
}