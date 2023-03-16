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
    private NetworkHandlerRemotePlayer _remotePlayer;
    private NetworkHandlerLocalPlayer _localPlayer;

    private Action _executeInMainThread;
    private bool _isSendingPlayerPostition;
    private bool _canUpdatePlayerInfo;

    private BotManager _botManager;

    public GameObject _playerPrefub;
    public string _playerName;

    async void Awake()
    {
        _playerName = $"player #{new System.Random().Next(1000)}";
        _remotePlayer = new NetworkHandlerRemotePlayer(_playerPrefub, ExecuteInMainThread);
        _localPlayer = new NetworkHandlerLocalPlayer(_playerPrefub, _playerName, ExecuteInMainThread);
        //_client = new ClientSideUnity("90.188.226.136", 4000, _localPlayer, _remotePlayer);
        _client = new ClientSideUnity("192.168.0.2", 4000, _localPlayer, _remotePlayer);
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
        _botManager = new BotManager(2, 5102);
        Task.Run(PostAwake);
    }

    async void OnDestroy()
    {
        if (_botManager.IsStarted)
            _botManager.StopSession();
        if (_localPlayer.PlayerInGame)
            await Disconnect();
        _client.Stop();
    }

    void Update()
    {
        _executeInMainThread?.Invoke();
        _executeInMainThread = null;
    }

    private Task PostAwake()
    {
        while (_client.IsStarted)
        {
            if (_executeInMainThread is null)
            {
                _executeInMainThread += async () =>
                {
                    if (_localPlayer.PlayerInGame && !_isSendingPlayerPostition)
                    {
                        _isSendingPlayerPostition = true;
                        StartCoroutine(SendPlayerPostitionTimeoutCoroutine(2f));
                        await SendPlayerPosition();
                    }
                    else if (!_localPlayer.PlayerInGame && _localPlayer.CanConnectToGame && _canUpdatePlayerInfo)
                    {
                        await ConnectToGame();
                        _canUpdatePlayerInfo = false;
                        StartCoroutine(UpdateInfoTimeoutCoroutine());
                    }
                    else if (!_localPlayer.CanConnectToGame && _canUpdatePlayerInfo)
                    {
                        _canUpdatePlayerInfo = false;
                        StartCoroutine(UpdateInfoTimeoutCoroutine());
                        await UpdatePlayerInfo();
                    }
                };
            }
            if (!_botManager.IsStarted && _localPlayer.PlayerInGame)
                _botManager.StartSession();
        }
        return Task.CompletedTask;
    }

    private void ExecuteInMainThread(Action action)
    {
        _executeInMainThread += action;
    }

    private async Task UpdatePlayerInfo()
    {
        GameNetworkPacket packet = new GameNetworkPacket
        {
            Player = _localPlayer.PlayerInfo,
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
        PlayerTransform transform = _localPlayer.GetPlayerTransform();
        GameNetworkObject networkObject = new GameNetworkObject()
        {
            Command = GameCommand.Move,
            CommandArgument = Serializer.GetJson(transform)
        };
        GameNetworkPacket packet = new GameNetworkPacket()
        {
            Player = _localPlayer.PlayerInfo,
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
        if (_localPlayer.PlayerInfo.IP is null)
            return;
        GameNetworkPacket packet = new GameNetworkPacket()
        {
            Player = _localPlayer.PlayerInfo,
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
            Player = _localPlayer.PlayerInfo,
            Command = GeneralCommand.Disconnect,
            Type = NetworkObjectType.None,
            Data = null
        };
        string json = Serializer.GetJson(packet);
        byte[] data = Encoding.ASCII.GetBytes(json);
        await _client.SendAsync(data);
    }
}