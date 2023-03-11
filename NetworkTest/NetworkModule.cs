using Mono.Cecil.Cil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;
using UnityEngine.UIElements;

public interface INetworkModule
{
    public Task ConnectToGame();
    public Task ConnectToGame(string code);
    public Task Disconnect();
}

public class NetworkModule : MonoBehaviour, INetworkModule
{
    private ClientSideUnity _client;
    private Action _executeInMainThread;
    private bool _isSendingPlayerPostition;

    public GameObject _playerPrefub;

    public NetworkHandlerRemotePlayer RemotePlayer { get; set; }
    public NetworkHandlerLocalPlayer LocalPlayer { get; set; }

    async void Awake()
    {
        RemotePlayer = new NetworkHandlerRemotePlayer(_playerPrefub, ExecuteInMainThread);
        LocalPlayer = new NetworkHandlerLocalPlayer(_playerPrefub, "Unity_Client_Test", ExecuteInMainThread);
        _client = new ClientSideUnity("194.28.155.220", 4000, LocalPlayer, RemotePlayer);
        _client.Logger.LogEvent += Debug.Log;
        _client.Logger.Level = LogLevel.Advanced;
        _client.Start();
        await UpdatePlayerInfo();
        await ConnectToGame();
        _isSendingPlayerPostition = false;
    }

    async void OnDestroy()
    {
        await Disconnect();
        _client.Stop();
    }

    async void Update()
    {
        _executeInMainThread?.Invoke();
        _executeInMainThread = null;
        if (LocalPlayer.PlayerInGame && !_isSendingPlayerPostition)
        {
            _isSendingPlayerPostition = true;
            StartCoroutine(SendPlayerPostitionTimeoutCoroutine(2f));
            await SendPlayerPosition();
        }
    }

    private void ExecuteInMainThread(Action action)
    {
        _executeInMainThread += action;
        Debug.Log("added new action");
    }

    private async Task UpdatePlayerInfo()
    {
        GameNetworkPacket packet = new GameNetworkPacket
        {
            Player = LocalPlayer.PlayerInfo,
            Command = GeneralCommand.PlayerInfo,
            Type = NetworkObjectType.None,
            Data = null
        };
        string json = Serializer.GetJson(packet);
        await _client.SendAsync(json);
    }

    private async Task SendPlayerPosition()
    {
        PlayerTransform transform = LocalPlayer.GetPlayerTransform();
        GameNetworkObject networkObject = new GameNetworkObject()
        {
            Command = GameCommand.Move,
            CommandArgument = Serializer.GetJson(transform)
        };
        GameNetworkPacket packet = new GameNetworkPacket()
        {
            Player = LocalPlayer.PlayerInfo,
            Command = GeneralCommand.GameCommand,
            Type = NetworkObjectType.GameNetworkObject,
            Data = Serializer.GetJson(networkObject)
        };
        string json = Serializer.GetJson(packet);
        await _client.SendAsync(json);
    }

    private IEnumerator SendPlayerPostitionTimeoutCoroutine(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        _isSendingPlayerPostition = false;
    }

    public Task ConnectToGame() => ConnectToGame(null);
    public async Task ConnectToGame(string code)
    {
        while (true)
        {
            if (LocalPlayer.PlayerInfo.IP is not null)
                break;
        }
        GameNetworkPacket packet = new GameNetworkPacket()
        {
            Player = LocalPlayer.PlayerInfo,
            Command = GeneralCommand.Connect,
            Type = NetworkObjectType.None,
            Data = code
        };
        string json = Serializer.GetJson(packet);
        await _client.SendAsync(json);
    }
    public async Task Disconnect()
    {
        GameNetworkPacket packet = new GameNetworkPacket()
        {
            Player = LocalPlayer.PlayerInfo,
            Command = GeneralCommand.Disconnect,
            Type = NetworkObjectType.None,
            Data = null
        };
        string json = Serializer.GetJson(packet);
        await _client.SendAsync(json);
    }
}

public class NetworkHandlerRemotePlayer
{
    private GameObject _playerPrefub;
    private Action<Action> _executeInMainThread;
    private List<GameObject> _remotePlayers;

    public NetworkHandlerRemotePlayer(GameObject playerPrefub, Action<Action> executeInMainThread)
    {
        _playerPrefub = playerPrefub;
        _executeInMainThread = executeInMainThread;
        _remotePlayers = new List<GameObject>(8);
    }

    // Можно не удалять объект, а просто скрывать и перемещать в начальную позицию

    public void Born(PlayerInfo playerInfo)
    {
        _executeInMainThread?.Invoke(() =>
        {
            GameObject player = GameObject.Instantiate<GameObject>(_playerPrefub);
            player.name = playerInfo.Name;
            _remotePlayers.Add(player);
        });
    }

    public void Dead(PlayerInfo playerInfo)
    {
        GameObject player = _remotePlayers.Find(x => x.name == playerInfo.Name);
        _executeInMainThread?.Invoke(() =>
        {
            GameObject.Destroy(player);
            _remotePlayers.Remove(player);
        });
    }

    public void Move(PlayerInfo playerInfo, string pos)
    {
        PlayerTransform transform = Serializer.GetObject<PlayerTransform>(pos);
        Vector3 position = new Vector3(transform.PositionX, transform.PositionY, transform.PositionZ);
        GameObject player = _remotePlayers.Find(x => x.name == playerInfo.Name);
        _executeInMainThread?.Invoke(() =>
        {
            player.transform.position = position;
            player.transform.rotation = new Quaternion(transform.RotationX, transform.RotationY, transform.RotationZ, 1);

        });
    }
}

public class NetworkHandlerLocalPlayer
{
    private GameObject _playerPrefub;
    private PlayerInfo _playerInfo;
    private Action<Action> _executeInMainThread;

    public NetworkHandlerLocalPlayer(GameObject playerPrefub, string playerName, Action<Action> executeInMainThread)
    {
        _playerPrefub = playerPrefub;
        _playerInfo = new PlayerInfo() { Name = playerName };
        _executeInMainThread = executeInMainThread;
    }

    public PlayerInfo PlayerInfo => _playerInfo;
    public bool PlayerInGame => PlayerInfo.GameCode != null;

    public void UpdatePlayerInfo(PlayerInfo playerInfo) =>
        _playerInfo = playerInfo;

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

    public void Connect()
    {
        _executeInMainThread?.Invoke(() =>
        {
            _playerPrefub = GameObject.Instantiate(_playerPrefub);
            _playerPrefub.name = _playerInfo.Name;
            Debug.Log("Player created");
        });
    }
}
