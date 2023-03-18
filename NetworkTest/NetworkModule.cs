using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface INetworkModule
{
    public Task ConnectToGame();
    public Task ConnectToGame(string code);
    public Task Disconnect();
}

public class NetworkModule : MonoBehaviour
{
    public GameObject _playerPrefub;
    public string _playerName;

    private ClientSideUnity _client;
    private NetworkHandlerLocalPlayer _localPlayer;
    private NetworkHandlerRemotePlayer _remotePlayer;
    private bool _canSendPlayerPosition;

    void Awake()
    {
        _playerName = $"Player #{DateTime.Now.Second}";
        _localPlayer = new NetworkHandlerLocalPlayer(_playerPrefub, _playerName);
        _remotePlayer = new NetworkHandlerRemotePlayer(_playerPrefub);
        InitClient();
        while (!_localPlayer.PlayerInGame)
        {
            if (_localPlayer.CanConnectToGame)
                _localPlayer.Connect(null);
            else
            {
                Thread.Sleep(2000);
                if (!_localPlayer.CanConnectToGame)
                    _localPlayer.UpdatePlayerInfo();
            }
            Thread.Sleep(2000);
        }
        _canSendPlayerPosition = true;
    }

    void OnDestroy()
    {
        if (_localPlayer.PlayerInGame)
            _localPlayer.Disconnect();
        _client.Stop();
    }

    async void Update()
    {
        if (_canSendPlayerPosition && _localPlayer.PlayerInGame)
        {
            await _localPlayer.SendPlayerPosition();
            StartCoroutine(SendPlayerPositionTimeout());
        }
        if (!_remotePlayer.IsTaskExecuting && _remotePlayer.CanExecuteTasks)
            ExecuteTasks(_remotePlayer);
        if (!_localPlayer.IsTaskExecuting && _localPlayer.CanExecuteTasks)
            ExecuteTasks(_localPlayer);
    }

    private void InitClient()
    {
        //_client = new ClientSideUnity("90.188.226.136", 4000, 5126, _localPlayer, _remotePlayer);
        _client = new ClientSideUnity("192.168.0.2", 4000, 5126, _localPlayer, _remotePlayer);
        _client.Logger.Level = LogLevel.Simple;
        _client.Logger.LogEvent += Debug.Log;
        _client.Start();
        _localPlayer.Client = _client;
        _localPlayer.UpdatePlayerInfo();
    }

    private void ExecuteTasks(ExecuteTasksInMainThread executor)
    {
        if (executor.Tasks.Count == 0
            || !executor.CanExecuteTasks)
            return;
        executor.IsTaskExecuting = true;
        foreach (var item in executor.Tasks)
        {
            if (!item.IsExecuted)
                item.Execute();
            item.IsExecuted = true;
        }
        executor.IsTaskExecuting = false;
    }

    private IEnumerator SendPlayerPositionTimeout()
    {
        _canSendPlayerPosition = false;
        yield return new WaitForSecondsRealtime(0.05f);
        _canSendPlayerPosition = true;
    }
}