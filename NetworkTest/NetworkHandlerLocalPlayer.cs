using System;
using UnityEngine;

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

    public PlayerInfo PlayerInfo { get => _playerInfo; set => _playerInfo = value; }
    public bool PlayerInGame => PlayerInfo.GameCode is not null;
    public bool CanConnectToGame => _playerInfo.IP is not null;

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
        });
    }
}

