using System.Collections.Generic;
using UnityEngine;

public class NetworkHandlerRemotePlayer : ExecuteTasksInMainThread
{
    private GameObject _playerPrefub;
    private List<GameObject> _remotePlayers;

    public NetworkHandlerRemotePlayer(GameObject playerPrefub)
    {
        _playerPrefub = playerPrefub;
        _remotePlayers = new List<GameObject>(8);
    }

    // Можно не удалять объект, а просто скрывать и перемещать в начальную позицию

    public void Born(PlayerInfo playerInfo)
    {
        ExecutableAction action = new ExecutableAction();
        action.Execute = () =>
        {
            if (_remotePlayers.Find(x => x.name == playerInfo.Name) is not null)
                return;
            GameObject player = GameObject.Instantiate<GameObject>(_playerPrefub);
            player.GetComponent<PlayerScript>()._isRemotePlayer = true;
            player.name = playerInfo.Name;
            _remotePlayers.Add(player);
        };
        base.AddTaskToQueue(action);
    }

    public void Dead(PlayerInfo playerInfo)
    {
        ExecutableAction action = new ExecutableAction();
        action.Execute = () =>
        {
            GameObject player = _remotePlayers.Find(x => x.name == playerInfo.Name);
            if (player is null)
                return;
            GameObject.Destroy(player);
            _remotePlayers.Remove(player);
        };
        base.AddTaskToQueue(action);
    }

    public void Move(PlayerInfo playerInfo, string pos)
    {
        ExecutableAction action = new ExecutableAction();
        action.Execute = () =>
        {
            PlayerTransform transform = Serializer.GetObject<PlayerTransform>(pos);
            Vector3 position = new Vector3(transform.PositionX, transform.PositionY, transform.PositionZ);
            GameObject player = _remotePlayers.Find(x => x.name == playerInfo.Name);
            player.transform.position = position;
            player.transform.rotation = new Quaternion(transform.RotationX, transform.RotationY, transform.RotationZ, 1);
        };
        base.AddTaskToQueue(action);
    }

    public void Attack(PlayerInfo playerInfo, string data)
    {

    }

    public void HittedAttack(PlayerInfo playerInfo, string data)
    {

    }
}


