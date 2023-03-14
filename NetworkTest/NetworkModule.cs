using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using System.Text;

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
    private bool _canUpdatePlayerInfo;

    public GameObject _playerPrefub;
    public string _playerName;
    public NetworkHandlerRemotePlayer RemotePlayer { get; set; }
    public NetworkHandlerLocalPlayer LocalPlayer { get; set; }

    async void Awake()
    {
        _playerName = $"player #{new System.Random().Next(1000)}";
        RemotePlayer = new NetworkHandlerRemotePlayer(_playerPrefub, ExecuteInMainThread);
        LocalPlayer = new NetworkHandlerLocalPlayer(_playerPrefub, _playerName, ExecuteInMainThread);
        _client = new ClientSideUnity("90.188.226.136", 4000, LocalPlayer, RemotePlayer);
        _client.Logger.LogEvent += Debug.Log;
        _client.Logger.Level = LogLevel.Advanced;
        _client.Start();
        _canUpdatePlayerInfo = true;
        if (_client.IsStarted)
        {
            _canUpdatePlayerInfo = false;
            await UpdatePlayerInfo();
            StartCoroutine(UpdateInfoTimeoutCoroutine());
        }
        _isSendingPlayerPostition = false;
    }

    async void OnDestroy()
    {
        if (LocalPlayer.PlayerInGame)
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
        else if (!LocalPlayer.PlayerInGame && LocalPlayer.CanConnectToGame)
        {
            await ConnectToGame();
        }
        else if (!LocalPlayer.CanConnectToGame && _canUpdatePlayerInfo)
        {
            _canUpdatePlayerInfo = false;
            StartCoroutine(UpdateInfoTimeoutCoroutine());
            await UpdatePlayerInfo();
        }
    }

    private void ExecuteInMainThread(Action action)
    {
        _executeInMainThread += action;
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
        byte[] data = Encoding.ASCII.GetBytes(json);
        await _client.SendAsync(data);
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
        byte[] data = Encoding.ASCII.GetBytes(json);
        await _client.SendAsync(data);
    }

    private IEnumerator SendPlayerPostitionTimeoutCoroutine(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        _isSendingPlayerPostition = false;
    }

    private IEnumerator UpdateInfoTimeoutCoroutine()
    {
        yield return new WaitForSecondsRealtime(5);
        _canUpdatePlayerInfo = true;
    }

    public Task ConnectToGame() => ConnectToGame(null);
    public async Task ConnectToGame(string code)
    {
        if (LocalPlayer.PlayerInfo.IP is null)
            return;
        GameNetworkPacket packet = new GameNetworkPacket()
        {
            Player = LocalPlayer.PlayerInfo,
            Command = GeneralCommand.Connect,
            Type = NetworkObjectType.None,
            Data = code
        };
        string json = Serializer.GetJson(packet);
        byte[] data = Encoding.ASCII.GetBytes(json);
        await _client.SendAsync(data);
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
        byte[] data = Encoding.ASCII.GetBytes(json);
        await _client.SendAsync(data);
    }
}