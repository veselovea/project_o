using System.Collections;
using System.Threading;
using UnityEngine;

public class NetworkModule : MonoBehaviour
{
    public GameObject _playerPrefub;
    public GameObject _remotePlayerPrefub;
    public string _playerName;

    private ClientSideUnity _client;
    private NetworkHandlerLocalPlayer _localPlayer;
    private NetworkHandlerRemotePlayer _remotePlayer;
    private BaseInitializer _baseInitializer;
    private bool _canSendPlayerPosition;

    async void Awake()
    {
        //_playerName = $"Player #{DateTime.Now.Second}";
        _baseInitializer = GameObject.Find("GameObject").GetComponent<BaseInitializer>();
        _localPlayer = new NetworkHandlerLocalPlayer(_playerPrefub, _playerName, _baseInitializer);
        _remotePlayer = new NetworkHandlerRemotePlayer(_playerPrefub, _baseInitializer);
        InitClient();
        while (!_localPlayer.PlayerInGame)
        {
            if (_localPlayer.CanConnectToGame)
                await _localPlayer.Connect(null);
            else
            {
                Thread.Sleep(2000);
                if (!_localPlayer.CanConnectToGame)
                    await _localPlayer.UpdatePlayerInfo();
            }
            Thread.Sleep(2000);
        }
        _canSendPlayerPosition = true;
    }

    async void OnDestroy()
    {
        if (_localPlayer.PlayerInGame)
            await _localPlayer.Disconnect();
        _client.Stop();
        _remotePlayer.OnDestroy();
    }

    async void Update()
    {
        if (_canSendPlayerPosition && _localPlayer.PlayerInGame)
        {
            _canSendPlayerPosition = false;
            await _localPlayer.SendPlayerPosition();
            StartCoroutine(SendPlayerPositionTimeout());
        }
        if (!_remotePlayer.IsTaskExecuting && _remotePlayer.CanExecuteTasks)
            _remotePlayer.ExecuteTasks();
        if (!_localPlayer.IsTaskExecuting && _localPlayer.CanExecuteTasks)
            _localPlayer.ExecuteTasks();
    }

    private async void InitClient()
    {
        //_client = new ClientSideUnity("90.188.226.136", 4000, 5126, _localPlayer, _remotePlayer);
        _client = new ClientSideUnity("192.168.0.5", 4000, 5126, _localPlayer, _remotePlayer);
        _client.Logger.Level = LogLevel.Simple;
        _client.Logger.LogEvent += Debug.Log;
        _client.Start();
        _localPlayer.Client = _client;
        await _localPlayer.UpdatePlayerInfo();
    }

    private IEnumerator SendPlayerPositionTimeout()
    {
        yield return new WaitForSecondsRealtime(0.05f);
        _canSendPlayerPosition = true;
    }
}