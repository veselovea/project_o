using System;
using System.Collections.Generic;
using UnityEngine;

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
            if (_remotePlayers.Find(x => x.name == playerInfo.Name) is not null)
                return;
            GameObject player = GameObject.Instantiate<GameObject>(_playerPrefub);
            player.GetComponent<PlayerScript>()._isRemotePlayer = true;
            player.name = playerInfo.Name;
            _remotePlayers.Add(player);
        });
    }

    public void Dead(PlayerInfo playerInfo)
    {
        _executeInMainThread?.Invoke(() =>
        {
            GameObject player = _remotePlayers.Find(x => x.name == playerInfo.Name);
            GameObject.Destroy(player);
            _remotePlayers.Remove(player);
        });
    }

    public void Move(PlayerInfo playerInfo, string pos)
    {
        PlayerTransform transform = Serializer.GetObject<PlayerTransform>(pos);
        _executeInMainThread?.Invoke(() =>
        {
            Debug.Log(pos);
            Vector3 position = new Vector3(transform.PositionX, transform.PositionY, transform.PositionZ);
            GameObject player = _remotePlayers.Find(x => x.name == playerInfo.Name);
            player.transform.position = position;
            player.transform.rotation = new Quaternion(transform.RotationX, transform.RotationY, transform.RotationZ, 1);
        });
    }
}


